using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IDeleteRuleCondition
{
    Task ExecuteAsync(Guid ruleConditionId, CancellationToken cancellationToken);
}

public class DeleteRuleCondition(AppDbContext dbContext) : IDeleteRuleCondition
{
    public async Task ExecuteAsync(Guid ruleConditionId, CancellationToken cancellationToken)
    {
        await dbContext.RuleConditions
            .Where(x => x.Id == ruleConditionId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}