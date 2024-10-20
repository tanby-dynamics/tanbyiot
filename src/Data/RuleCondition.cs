using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class RuleCondition
{
    public Guid Id { get; init; }
    public Guid RuleId { get; init; }
    public RuleConditionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public TelemetryTypeMatchingType? TelemetryTypeMatchingType { get; set; }
    [MaxLength(128)] public string? StateKey { get; set; }
    [MaxLength(128)] public string? ComparisonValue { get; set; }
    public RuleConditionComparisonOperationType? ComparisonOperation { get; set; }
    [MaxLength(128)] public string? PayloadPath { get; set; }
    public RuleConditionConversionType? Conversion { get; set; }
    public virtual Rule Rule { get; init; } = default!;
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}