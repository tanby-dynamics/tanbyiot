using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IUpdateRuleCondition
{
    Task<RuleConditionDto> ExecuteAsync(Guid ruleId, Guid ruleConditionId, string? comparisonValue,
        RuleConditionComparisonOperationType? comparisonOperation, string? payloadPath,
        RuleConditionConversionType? conversion, CancellationToken cancellationToken);
}

public class UpdateRuleCondition(AppDbContext dbContext) : IUpdateRuleCondition
{
    public async Task<RuleConditionDto> ExecuteAsync(Guid ruleId, Guid ruleConditionId, string? comparisonValue,
        RuleConditionComparisonOperationType? comparisonOperation, string? payloadPath,
        RuleConditionConversionType? conversion, CancellationToken cancellationToken)
    {
        var condition =
            await dbContext.RuleConditions.SingleAsync(x => x.RuleId == ruleId && x.Id == ruleConditionId,
                cancellationToken);

        condition.ComparisonValue = comparisonValue;
        condition.ComparisonOperation = comparisonOperation;
        condition.PayloadPath = payloadPath;
        condition.Conversion = conversion;

        await dbContext.SaveChangesAsync(cancellationToken);

        return RuleConditionDto.FromEntity(condition);
    }
}