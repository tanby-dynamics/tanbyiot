using Data;

namespace Services.Rules.Conditions;

public class UpdateRuleConditionArgs
{
    public Guid RuleConditionId { get; init; }
    public string? Description { get; init; } = default!;
    
    public string? ApplicationStateMatchingKey { get; init; }
    public ApplicationStateMatchingType? ApplicationStateMatchingType { get; init; }
    public string? ApplicationStateMatchingValue { get; init; }
    public string? ApplicationStateMatchingPayloadPath { get; init; }
    public ComparisonOperationType? ApplicationStateComparisonOperationType { get; init; }
    
    public DeviceMatchingType? DeviceMatchingType { get; init; }
    public Guid? DeviceMatchingId { get; init; }
    public string? DeviceMatchingGroups { get; init; }
    
    public TelemetryTypeMatchingType TelemetryTypeMatchingType { get; init; }
    public string? TelemetryTypeMatchingSpecifiedTypes{ get; init; }
    
    public TelemetryValueMatchingType? TelemetryValueMatchingType { get; init; }
    public string? TelemetryValueMatchingPayloadPath { get; init; }
    public ComparisonOperationType? TelemetryValueMatchingComparisonOperationType { get; init; }
    public string? TelemetryValueMatchingValue { get; init; }
}