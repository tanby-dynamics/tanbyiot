using Microsoft.AspNetCore.Mvc;
using Services.Devices;
using Services.Instructions;
using Services.Telemetries;

namespace tanbyiot.Server.Features.Hw;

[ApiController]
[Route("/api/hw")]
public class HwController(
    IAddTelemetry addTelemetry,
    IConnectDevice connectDevice,
    IPollForInstructions pollForInstructions) : ControllerBase
{
    [HttpPost("add-telemetry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTelemetry(AddTelemetryArgsDto args, CancellationToken cancellationToken)
    {
        await addTelemetry.ExecuteAsync(args, cancellationToken);

        return Ok();
    }
   
    [HttpPost("connect-device")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConnectDevice(ConnectDeviceArgsDto args, CancellationToken cancellationToken)
    {
        await connectDevice.ExecuteAsync(args.DeviceId, cancellationToken);

        return Ok();
    }
    
    [HttpPost("poll-for-instructions")]
    [ProducesResponseType<IEnumerable<PollForInstructionsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> PollForInstructions(
        PollForInstructionsArgsDto args,
        CancellationToken cancellationToken)
    {
        var response = await pollForInstructions.ExecuteAsync(args.DeviceId, cancellationToken);

        return Ok(response);
    }
}