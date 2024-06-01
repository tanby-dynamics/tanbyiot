using Data;

namespace Services.Rules;

public class RuleConditionDto
{
    public Guid Id { get; set; }
    public RuleConditionType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ComparisonValue { get; set; }
    public RuleConditionComparisonOperationType? ComparisonOperation { get; set; }
    public string? PayloadPath { get; set; }
    public RuleConditionConversionType? Conversion { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public static RuleConditionDto FromEntity(RuleCondition condition)
    {
        return new RuleConditionDto
        {
            Id = condition.Id,
            Type = condition.Type,
            CreatedAt = condition.CreatedAt,
            Description = $"Description for {condition.Type} condition",
            ComparisonValue = condition.ComparisonValue,
            ComparisonOperation = condition.ComparisonOperation,
            PayloadPath = condition.PayloadPath,
            Conversion = condition.Conversion,
            UpdatedAt = condition.UpdatedAt
        };
    }
}