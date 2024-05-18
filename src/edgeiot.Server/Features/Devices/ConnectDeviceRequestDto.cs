namespace edgeiot.Server.Features.Devices;

public class ConnectDeviceRequestDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
}