using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Processing;

public class ProcessSetStateAction(AppDbContext dbContext, ISystemClock clock) : IProcessAction
{
    public async Task ExecuteAsync(RuleAction action, TenantContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<ProcessSetStateAction>();

        if (action.Key is null)
        {
            log.Error(
                "Key is null for set state action {RuleActionId} in tenant {TenantId}",
                action.Id,
                context.Tenant.Id);
            return;
        }

        var state = await dbContext.TenantStates.SingleOrDefaultAsync(
                        x => x.TenantId == context.Tenant.Id && x.Key == action.Key,
                        cancellationToken)
                    ?? (await dbContext.TenantStates.AddAsync(new TenantState
                    {
                        TenantId = context.Tenant.Id,
                        Key = action.Key
                    }, cancellationToken)).Entity;

        state.Value = action.Payload ?? string.Empty;
        state.SetAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        
        log.Information("Set tenant state for key {Key} to value {Value} for tenant {TenantId}",
            state.Key,
            state.Value,
            state.TenantId);
    }
}