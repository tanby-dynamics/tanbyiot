using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.Rules.Conditions;

public interface IDeleteRuleCondition
{
    Task ExecuteAsync(Guid ruleConditionId, CancellationToken cancellationToken);
}

public class DeleteRuleCondition(AppDbContext dbContext, ISystemClock clock) : IDeleteRuleCondition
{
    public async Task ExecuteAsync(Guid ruleConditionId, CancellationToken cancellationToken)
    {
        var ruleCondition = await dbContext.RuleConditions
            .SingleAsync(x => x.Id == ruleConditionId, cancellationToken);

        ruleCondition.DeletedAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}