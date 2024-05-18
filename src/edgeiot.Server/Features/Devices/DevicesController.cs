using Microsoft.AspNetCore.Mvc;
using Services.Devices;

namespace edgeiot.Server.Features.Devices;

[ApiController]
[Route("/api/devices")]
public class DevicesController(
    IGetAllDevicesForTenant getAllDevicesForTenant, 
    IAddDevice addDevice,
    IValidateDevice validateDevice,
    IConnectDevice connectDevice) : ControllerBase
{
    private static Guid TenantId => Guid.Parse("de37f1e6-70a1-4c69-bdbc-317ff86b5267"); // Hard-coded to our single initial test tenant
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<DeviceDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var devices = await getAllDevicesForTenant.ExecuteAsync(
            TenantId,
            cancellationToken);
        
        return Ok(devices);
    }

    [HttpPost]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddDevice(AddDeviceRequestDto request, CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var device = await addDevice.ExecuteAsync(TenantId, request.Name, request.GroupName, cancellationToken);

        return Ok(device);
    }
    
    [HttpPost("connect")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConnectDevice(ConnectDeviceRequestDto requestDto, CancellationToken cancellationToken)
    {
        var isDeviceValidated = await validateDevice.ExecuteAsync(requestDto.TenantId, requestDto.DeviceId, cancellationToken);

        if (!isDeviceValidated)
        {
            return BadRequest("Device is invalid");
        }
        
        await connectDevice.ExecuteAsync(requestDto.TenantId, requestDto.DeviceId, cancellationToken);

        return Ok();
    }
}