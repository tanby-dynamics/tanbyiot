using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Processing;

public interface IProcessTelemetry
{
    Task ExecuteAsync(Guid telemetryId, CancellationToken cancellationToken);
}

public class ProcessTelemetry(AppDbContext dbContext, ISystemClock clock, IApplicationContextFactory applicationContextFactory,
    IProcessRule processRule) : IProcessTelemetry
{
    public async Task ExecuteAsync(Guid telemetryId, CancellationToken cancellationToken)
    {
        var telemetry = await dbContext.Telemetries.SingleAsync(x => x.Id == telemetryId, cancellationToken);
        var log = Log.ForContext<ProcessTelemetry>();

        if (telemetry.ProcessedAt is not null)
        {
            log.Warning("Already processed telemetry {TelemetryId}", telemetryId);
            return;
        }

        var enabledRules = dbContext.Rules
            .Include(x => x.Conditions)
            .Include(x => x.Actions)
            .Where(x => x.Enabled);
        var context = applicationContextFactory.CreateNew();
        
        context.CurrentTelemetry = telemetry;

        foreach (var rule in enabledRules)
        {
            await processRule.ExecuteAsync(rule, context, cancellationToken);
        }
        
        telemetry.ProcessedAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}