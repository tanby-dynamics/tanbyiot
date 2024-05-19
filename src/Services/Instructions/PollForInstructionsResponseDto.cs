namespace Services.Instructions;

public class PollForInstructionsResponseDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Payload { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}