using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Device : ISoftDelete
{
    public Guid Id { get; init; }
    [MaxLength(128)] public string Name { get; init; } = string.Empty;
    [MaxLength(128)] public string GroupName { get; init; } = string.Empty;
    public DateTimeOffset? LastConnected { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public virtual ICollection<Telemetry> Telemetries { get; init; } = default!;
    public virtual ICollection<Instruction> Instructions { get; init; } = default!;
}