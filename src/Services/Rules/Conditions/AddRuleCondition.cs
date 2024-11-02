using Data;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Rules.Conditions;

public interface IAddRuleCondition
{
    Task<RuleConditionDto> ExecuteAsync(Guid ruleId, RuleConditionType type, CancellationToken cancellationToken);
}

public class AddRuleCondition(AppDbContext dbContext, ISystemClock clock) : IAddRuleCondition
{
    public async Task<RuleConditionDto> ExecuteAsync(Guid ruleId, RuleConditionType type, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<AddRuleCondition>();

        var newRuleCondition = new RuleCondition
        {
            RuleId = ruleId,
            Type = type,
            CreatedAt = clock.UtcNow
        };

        if (type == RuleConditionType.Telemetry)
        {
            // Set some defaults for the telemetry type
            newRuleCondition.DeviceMatchingType = DeviceMatchingType.AllDevices;
            newRuleCondition.TelemetryTypeMatchingType = TelemetryTypeMatchingType.AllTypes;
            newRuleCondition.TelemetryValueMatchingType = TelemetryValueMatchingType.AlwaysMatch;
            newRuleCondition.TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.Equals;
        }
        
        var result = await dbContext.RuleConditions.AddAsync(newRuleCondition, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        log.Information("Added condition {RuleConditionId} for rule {RuleId}", result.Entity.Id, ruleId);

        return RuleConditionDto.FromEntity(result.Entity);
    }
}