using Data;

namespace Services.Rules;

public class RuleConditionDto
{
    public Guid Id { get; init; }
    public RuleConditionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? ComparisonValue { get; init; }
    public RuleConditionComparisonOperationType? ComparisonOperation { get; init; }
    public string? PayloadPath { get; init; }
    public RuleConditionConversionType? Conversion { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }

    public static RuleConditionDto FromEntity(RuleCondition condition)
    {
        var telemetryTypes = (condition.ComparisonValue ?? string.Empty)
            .Split(",")
            .Select(x => x.Trim())
            .ToArray();
        var description = condition.Type switch
        {
            RuleConditionType.TelemetryTypes => string.Join(", ", telemetryTypes),
            _ => string.Empty
        };
        
        return new RuleConditionDto
        {
            Id = condition.Id,
            Type = condition.Type,
            CreatedAt = condition.CreatedAt,
            Description = description,
            ComparisonValue = condition.ComparisonValue,
            ComparisonOperation = condition.ComparisonOperation,
            PayloadPath = condition.PayloadPath,
            Conversion = condition.Conversion,
            UpdatedAt = condition.UpdatedAt
        };
    }
}