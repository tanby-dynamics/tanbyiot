using Microsoft.AspNetCore.Mvc;
using Services;

namespace Api.Features.Devices;

[ApiController]
[Route("/api/devices")]
public class DevicesController : ControllerBase
{
    private IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet("get-new-api-key")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> GetNewApiKey(CancellationToken cancellationToken)
    {
        var apiKey = await _deviceService.GetNewApiKey("TENANT", cancellationToken);
        
        return Ok(apiKey);
    }
}