using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IValidateActionInRule
{
    Task<bool> ExecuteAsync(Guid ruleId, Guid actionId, CancellationToken cancellationToken);
}

public class ValidateActionInRule(AppDbContext dbContext) : IValidateActionInRule
{
    public async Task<bool> ExecuteAsync(Guid ruleId, Guid actionId, CancellationToken cancellationToken)
    {
        return await dbContext.RuleActions.AnyAsync(x => x.RuleId == ruleId && x.Id == actionId,
            cancellationToken);
    }
}