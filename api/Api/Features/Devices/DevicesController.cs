using Microsoft.AspNetCore.Mvc;
using Services;

namespace Api.Features.Devices;

[ApiController]
[Route("/api/devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

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

    [HttpPost("connect")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConnectResponseDto))]
    public Task<IActionResult> Connect(
        [FromBody] ConnectRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = new ConnectResponseDto { Token = "deadbeef" };

        return Task.FromResult<IActionResult>(Ok(response));
    }
}

public record ConnectRequestDto
{
    public string ApiKey { get; set; } = string.Empty;
}

public record ConnectResponseDto
{
    public string Token { get; set; } = string.Empty;
}