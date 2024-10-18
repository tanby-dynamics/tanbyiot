using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Telemetry
{
    public Guid Id { get; init; }
    public Guid DeviceId { get; init; }
    // ReSharper disable once EntityFramework.ModelValidation.CircularDependency
    public virtual Device Device { get; init; } = default!;
    [MaxLength(128)] public string Type { get; init; } = string.Empty;
    [MaxLength(128)] public string? Value { get; init; }
    [MaxLength(4000)] public string? Payload { get; init; } = string.Empty;
    public DateTimeOffset ReceivedAt { get; init; }
    public DateTimeOffset? ProcessedAt { get; set; }
}