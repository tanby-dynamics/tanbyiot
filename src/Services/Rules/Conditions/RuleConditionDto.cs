using Data;

namespace Services.Rules.Conditions;

public class RuleConditionDto
{
    public Guid Id { get; init; }
    public RuleConditionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public string? Description { get; init; }

    public string? ApplicationStateMatchingKey { get; init; }
    public ApplicationStateMatchingType? ApplicationStateMatchingType { get; init; }
    public string? ApplicationStateMatchingValue { get; init; }
    public string? ApplicationStateMatchingPayloadPath { get; init; }
    public ComparisonOperationType? ApplicationStateComparisonOperationType { get; init; }
    
    public DeviceMatchingType? DeviceMatchingType { get; init; }
    public Guid? DeviceMatchingId { get; init; }
    public string? DeviceMatchingGroups { get; init; }
    
    public TelemetryTypeMatchingType? TelemetryTypeMatchingType { get; init; }
    public string? TelemetryTypeMatchingSpecifiedTypes { get; init; }

    public TelemetryValueMatchingType? TelemetryValueMatchingType { get; init; }
    public string? TelemetryValueMatchingPayloadPath { get; init; }
    public ComparisonOperationType? TelemetryValueMatchingComparisonOperationType { get; init; }
    public string? TelemetryValueMatchingValue { get; init; }

    public static RuleConditionDto FromEntity(RuleCondition condition)
    {
        return new RuleConditionDto
        {
            Id = condition.Id,
            Type = condition.Type,
            CreatedAt = condition.CreatedAt,
            UpdatedAt = condition.UpdatedAt,
            Description = condition.Description,
            ApplicationStateMatchingKey = condition.ApplicationStateMatchingKey,
            ApplicationStateMatchingType = condition.ApplicationStateMatchingType,
            ApplicationStateMatchingValue = condition.ApplicationStateMatchingValue,
            ApplicationStateMatchingPayloadPath = condition.ApplicationStateMatchingPayloadPath,
            ApplicationStateComparisonOperationType = condition.ApplicationStateComparisonOperationType,
            DeviceMatchingType =  condition.DeviceMatchingType,
            DeviceMatchingId = condition.DeviceMatchingId,
            DeviceMatchingGroups = condition.DeviceMatchingGroups,
            TelemetryTypeMatchingType = condition.TelemetryTypeMatchingType,
            TelemetryTypeMatchingSpecifiedTypes = condition.TelemetryTypeMatchingSpecifiedTypes,
            TelemetryValueMatchingType = condition.TelemetryValueMatchingType,
            TelemetryValueMatchingPayloadPath = condition.TelemetryValueMatchingPayloadPath,
            TelemetryValueMatchingComparisonOperationType = condition.TelemetryValueMatchingComparisonOperationType,
            TelemetryValueMatchingValue = condition.TelemetryValueMatchingValue
        };
    }
}