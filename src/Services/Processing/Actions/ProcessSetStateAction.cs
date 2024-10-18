using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Processing.Actions;

public class ProcessSetStateAction(AppDbContext dbContext, ISystemClock clock) : IProcessAction
{
    public async Task ExecuteAsync(RuleAction action, ApplicationContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<ProcessSetStateAction>();

        if (action.Key is null)
        {
            log.Error(
                "Key is null for set state action {RuleActionId}",
                action.Id);
            return;
        }

        var state = await dbContext.ApplicationStates.SingleOrDefaultAsync(
                        x => x.Key == action.Key,
                        cancellationToken)
                    ?? (await dbContext.ApplicationStates.AddAsync(new ApplicationState
                    {
                        Key = action.Key
                    }, cancellationToken)).Entity;

        state.Value = action.Payload ?? string.Empty;
        state.SetAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        
        log.Information("Set application state for key {Key} to value {Value}",
            state.Key,
            state.Value);
    }
}