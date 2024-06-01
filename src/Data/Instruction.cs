using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Instruction
{
    public Guid Id { get; init; }
    public Guid DeviceId { get; init; }
    public virtual Device Device { get; init; } = default!;
    [MaxLength(128)] public string Type { get; init; } = string.Empty;
    [MaxLength(128)] public string? Value { get; init; }
    [MaxLength(4000)] public string? Payload { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? SentAt { get; set; }
}