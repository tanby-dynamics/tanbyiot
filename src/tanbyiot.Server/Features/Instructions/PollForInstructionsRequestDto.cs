namespace tanbyiot.Server.Features.Instructions;

public class PollForInstructionsRequestDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
}