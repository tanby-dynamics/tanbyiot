using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.Rules.Actions;

public interface IDeleteRuleAction
{
    Task ExecuteAsync(Guid ruleActionId, CancellationToken cancellationToken);
}

public class DeleteRuleAction(AppDbContext dbContext, ISystemClock clock) : IDeleteRuleAction
{
    public async Task ExecuteAsync(Guid ruleActionId, CancellationToken cancellationToken)
    {
        var ruleAction = await dbContext.RuleActions
            .SingleAsync(x => x.Id == ruleActionId, cancellationToken);

        ruleAction.DeletedAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}