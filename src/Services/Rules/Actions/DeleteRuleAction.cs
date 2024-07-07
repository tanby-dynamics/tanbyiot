using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules.Actions;

public interface IDeleteRuleAction
{
    Task ExecuteAsync(Guid ruleActionId, CancellationToken cancellationToken);
}

public class DeleteRuleAction(AppDbContext dbContext) : IDeleteRuleAction
{
    public async Task ExecuteAsync(Guid ruleActionId, CancellationToken cancellationToken)
    {
        await dbContext.RuleActions
            .Where(x => x.Id == ruleActionId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}