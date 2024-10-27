using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class RuleCondition
{
    public Guid Id { get; init; }
    public Guid RuleId { get; init; }
    public virtual Rule Rule { get; init; } = default!;
    public RuleConditionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    [MaxLength(300)] public string Description { get; set; } = default!;
    
    [MaxLength(128)] public string? ApplicationStateMatchingKey { get; set; }
    public ApplicationStateMatchingType? ApplicationStateMatchingType { get; set; }
    [MaxLength(128)] public string? ApplicationStateMatchingValue { get; set; }
    [MaxLength(128)] public string? ApplicationStateMatchingPayloadPath { get; set; }
    public ComparisonOperationType? ApplicationStateComparisonOperationType { get; set; }


    public DeviceMatchingType? DeviceMatchingType { get; set; }
    public Guid? DeviceMatchingId { get; set; }
    [MaxLength(128)] public string? DeviceMatchingGroups { get; set; }
    
    public TelemetryTypeMatchingType? TelemetryTypeMatchingType { get; set; }
    [MaxLength(128)] public string? TelemetryTypeMatchingSpecifiedTypes { get; set; }

    public TelemetryValueMatchingType? TelemetryValueMatchingType { get; set; }
    [MaxLength(128)] public string? TelemetryValueMatchingPayloadPath { get; set; }
    public ComparisonOperationType? TelemetryValueMatchingComparisonOperationType { get; set; }
}