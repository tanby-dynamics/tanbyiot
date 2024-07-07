using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules.Conditions;

public interface IValidateConditionInRule
{
    Task<bool> ExecuteAsync(Guid ruleId, Guid conditionId, CancellationToken cancellationToken);
}

public class ValidateConditionInRule(AppDbContext dbContext) : IValidateConditionInRule
{
    public async Task<bool> ExecuteAsync(Guid ruleId, Guid conditionId, CancellationToken cancellationToken)
    {
        return await dbContext.RuleConditions.AnyAsync(x => x.RuleId == ruleId && x.Id == conditionId,
            cancellationToken);
    }
}