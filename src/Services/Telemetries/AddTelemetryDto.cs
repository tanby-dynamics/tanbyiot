using System.ComponentModel.DataAnnotations;

namespace Services.Telemetries;

public class AddTelemetryDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
    [MaxLength(128)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(128)]
    public string Value { get; set; } = string.Empty;
    [MaxLength(4000)]
    public string Payload { get; set; } = string.Empty;
}