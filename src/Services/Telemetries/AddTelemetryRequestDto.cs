using System.ComponentModel.DataAnnotations;

namespace Services.Telemetries;

public class AddTelemetryRequestDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
    [MaxLength(128)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(128)]
    public string? Value { get; set; }
    [MaxLength(4000)]
    public string? Payload { get; set; }
}