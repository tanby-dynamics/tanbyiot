using Microsoft.AspNetCore.Mvc;
using Services.Devices;

namespace edgeiot.Server.Features;

[ApiController]
[Route("/api/devices")]
public class DevicesController(IGetAllDevicesForTenant getAllDevicesForTenant) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<DeviceDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        
        var devices = await getAllDevicesForTenant.ExecuteAsync(
            Guid.Parse("de37f1e6-70a1-4c69-bdbc-317ff86b5267"), // Hard-coded to our single initial test tenant
            cancellationToken);
        
        return Ok(devices);
    }
}
