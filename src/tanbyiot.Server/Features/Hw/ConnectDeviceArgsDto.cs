namespace tanbyiot.Server.Features.Hw;

public class ConnectDeviceArgsDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
}