using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IDeleteRuleCondition
{
    Task ExecuteAsync(Guid ruleId, Guid ruleConditionId, CancellationToken cancellationToken);
}

public class DeleteRuleCondition(AppDbContext dbContext) : IDeleteRuleCondition
{
    public async Task ExecuteAsync(Guid ruleId, Guid ruleConditionId, CancellationToken cancellationToken)
    {
        await dbContext.RuleConditions
            .Where(x => x.RuleId == ruleId && x.Id == ruleConditionId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}