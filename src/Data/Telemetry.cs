using System.ComponentModel.DataAnnotations;

namespace Data;

public class Telemetry
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public virtual Tenant Tenant { get; set; }
    public Guid DeviceId { get; init; }
    public virtual Device Device { get; set; }
    [MaxLength(128)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(128)]
    public string Value { get; set; } = string.Empty;
    [MaxLength(4000)]
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset ReceivedAt { get; set; }
}