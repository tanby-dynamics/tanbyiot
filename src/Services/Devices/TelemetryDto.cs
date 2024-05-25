namespace Services.Devices;

public class TelemetryDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Payload { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
}