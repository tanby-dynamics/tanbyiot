namespace Services.Devices;

public class GetInstructionsForDeviceResponseDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
}