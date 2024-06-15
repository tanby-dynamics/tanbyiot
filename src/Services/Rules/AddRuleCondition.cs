using Data;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Rules;

public interface IAddRuleCondition
{
    Task<RuleConditionDto> ExecuteAsync(Guid ruleId, RuleConditionType type, CancellationToken cancellationToken);
}

public class AddRuleCondition(AppDbContext dbContext, ISystemClock clock) : IAddRuleCondition
{
    public async Task<RuleConditionDto> ExecuteAsync(Guid ruleId, RuleConditionType type, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<AddRuleCondition>();
        var result = await dbContext.RuleConditions.AddAsync(new RuleCondition
        {
            RuleId = ruleId,
            Type = type,
            CreatedAt = clock.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        log.Information("Added condition {RuleConditionId} for rule {RuleId}", result.Entity.Id, ruleId);

        return RuleConditionDto.FromEntity(result.Entity);
    }
}