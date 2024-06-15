using Microsoft.AspNetCore.Mvc;
using Services.Devices;
using Services.Telemetries;

namespace tanbyiot.Server.Features.Telemetries;

[ApiController]
[Route("/api/telemetry")]
public class TelemetryController(IValidateDevice validateDevice, IAddTelemetry addTelemetry) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTelemetry(AddTelemetryRequestDto requestDto, CancellationToken cancellationToken)
    {
        var isDeviceValidated = await validateDevice.ExecuteAsync(requestDto.TenantId, requestDto.DeviceId, cancellationToken);

        if (!isDeviceValidated)
        {
            return BadRequest("Device is invalid");
        }
        
        await addTelemetry.ExecuteAsync(requestDto, cancellationToken);

        return Ok();
    }
}

