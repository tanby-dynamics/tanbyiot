using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IDeleteRuleAction
{
    Task ExecuteAsync(Guid ruleId, Guid ruleActionId, CancellationToken cancellationToken);
}

public class DeleteRuleAction(AppDbContext dbContext) : IDeleteRuleAction
{
    public async Task ExecuteAsync(Guid ruleId, Guid ruleActionId, CancellationToken cancellationToken)
    {
        await dbContext.RuleActions
            .Where(x => x.RuleId == ruleId && x.Id == ruleActionId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}