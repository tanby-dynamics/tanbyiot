using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Instruction
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public virtual Device Device { get; set; }
    [MaxLength(128)] public string Type { get; set; } = string.Empty;
    [MaxLength(128)] public string Value { get; set; } = string.Empty;
    [MaxLength(4000)] public string? Payload { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
}