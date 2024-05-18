using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Services.Devices;
using Services.Telemetries;

namespace edgeiot.Server.Features.Telemetries;

[ApiController]
[Route("/api/telemetry")]
public class TelemetryController(IValidateDevice validateDevice, IAddTelemetry addTelemetry) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTelemetry(AddTelemetryDto dto, CancellationToken cancellationToken)
    {
        var isDeviceValidated = await validateDevice.ExecuteAsync(dto.TenantId, dto.DeviceId, cancellationToken);

        if (!isDeviceValidated)
        {
            return BadRequest("Device is invalid");
        }
        
        await addTelemetry.ExecuteAsync(dto, cancellationToken);

        return Ok();
    }
}

