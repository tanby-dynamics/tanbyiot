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

        condition.ComparisonValue = args.ComparisonValue;
        condition.TelemetryValueMatchingPayloadPath = args.PayloadPath;
        condition.Conversion = args.Conversion;
        condition.ApplicationStateMatchingKey = args.Key;
        condition.TelemetryMatchingType = args.TelemetryTypeMatchingType;
        condition.ApplicationStateComparisonOperationType = args.ApplicationValueStateComparisonOperationType;

        await dbContext.SaveChangesAsync(cancellationToken);

        return RuleConditionDto.FromEntity(condition);
    }
}