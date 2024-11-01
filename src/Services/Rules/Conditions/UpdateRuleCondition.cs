using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules.Conditions;

public interface IUpdateRuleCondition
{
    Task<RuleConditionDto> ExecuteAsync(
        UpdateRuleConditionArgs args,
        CancellationToken cancellationToken);
}

public class UpdateRuleCondition(AppDbContext dbContext) : IUpdateRuleCondition
{
    public async Task<RuleConditionDto> ExecuteAsync(
        UpdateRuleConditionArgs args,
        CancellationToken cancellationToken)
    {
        var condition = await dbContext.RuleConditions
            .SingleAsync(x => x.Id == args.RuleConditionId, cancellationToken);

        condition.Description = args.Description;
        condition.ApplicationStateMatchingKey = args.ApplicationStateMatchingKey;
        condition.ApplicationStateMatchingType = args.ApplicationStateMatchingType;
        condition.ApplicationStateMatchingValue = args.ApplicationStateMatchingValue;
        condition.ApplicationStateMatchingPayloadPath = args.ApplicationStateMatchingPayloadPath;
        condition.ApplicationStateComparisonOperationType = args.ApplicationStateComparisonOperationType;
        condition.DeviceMatchingType = args.DeviceMatchingType;
        condition.DeviceMatchingId = args.DeviceMatchingId;
        condition.DeviceMatchingGroups = args.DeviceMatchingGroups;
        condition.TelemetryTypeMatchingType = args.TelemetryTypeMatchingType;
        condition.TelemetryTypeMatchingSpecifiedTypes = args.TelemetryTypeMatchingSpecifiedTypes;
        condition.TelemetryValueMatchingType = args.TelemetryValueMatchingType;
        condition.TelemetryValueMatchingPayloadPath  = args.TelemetryValueMatchingPayloadPath;
        condition.TelemetryValueMatchingComparisonOperationType  = args.TelemetryValueMatchingComparisonOperationType;
        condition.TelemetryValueMatchingValue  = args.TelemetryValueMatchingValue;

        await dbContext.SaveChangesAsync(cancellationToken);

        return RuleConditionDto.FromEntity(condition);
    }
}