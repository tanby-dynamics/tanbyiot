using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Processing;

public interface IProcessTelemetry
{
    Task ExecuteAsync(Guid telemetryId, CancellationToken cancellationToken);
}

public class ProcessTelemetry(AppDbContext dbContext, ISystemClock clock, ITenantContextFactory tenantContextFactory,
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

        var tenant = await dbContext.Tenants
            .Include(x => x.Rules).ThenInclude(x => x.Conditions)
            .Include(x => x.Rules).ThenInclude(x => x.Actions)
            .SingleAsync(x => x.Id == telemetry.TenantId, cancellationToken);

        var context = tenantContextFactory.CreateNew(tenant);

        context.CurrentTelemetry = telemetry;

        foreach (var rule in tenant.Rules.Where(x => x.Enabled))
        {
            await processRule.ExecuteAsync(rule, context, cancellationToken);
        }
        
        telemetry.ProcessedAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}