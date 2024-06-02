using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class RuleAction : ISoftDelete, IUpdatable
{
    public Guid Id { get; init; }
    public Guid RuleId { get; init; }
    public RuleActionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    [MaxLength(128)] public string? SendInstructionType { get; set; }
    [MaxLength(128)] public string? SendInstructionValue { get; set; }
    [MaxLength(4000)] public string? SendInstructionPayload { get; set; }
    public Guid? SendInstructionDeviceId { get; set; }
    [MaxLength(256)] public string? SendInstructionDeviceGroups { get; set; }
    public virtual Rule Rule { get; init; } = default!;
}