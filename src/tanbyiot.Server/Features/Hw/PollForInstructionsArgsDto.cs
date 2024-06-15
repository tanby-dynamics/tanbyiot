namespace tanbyiot.Server.Features.Hw;

public class PollForInstructionsArgsDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
}