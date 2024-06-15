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
    IPollForInstructions pollForInstructions,
    IValidateDeviceInTenant validateDeviceInTenant) : ControllerBase
{
    [HttpPost("add-telemetry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTelemetry(AddTelemetryArgsDto args, CancellationToken cancellationToken)
    {
        var isDeviceInTenant = await validateDeviceInTenant.ExecuteAsync(args.TenantId, args.DeviceId, cancellationToken);

        if (!isDeviceInTenant)
        {
            return BadRequest("Device is invalid");
        }
        
        await addTelemetry.ExecuteAsync(args, cancellationToken);

        return Ok();
    }
   
    [HttpPost("connect-device")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConnectDevice(ConnectDeviceArgsDto args, CancellationToken cancellationToken)
    {
        var isDeviceInTenant = await validateDeviceInTenant.ExecuteAsync(args.TenantId, args.DeviceId, cancellationToken);

        if (!isDeviceInTenant)
        {
            return BadRequest("Device is invalid");
        }
        
        await connectDevice.ExecuteAsync(args.TenantId, args.DeviceId, cancellationToken);

        return Ok();
    }
    
    [HttpPost("poll-for-instructions")]
    [ProducesResponseType<IEnumerable<PollForInstructionsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> PollForInstructions(
        PollForInstructionsArgsDto args,
        CancellationToken cancellationToken)
    {
        var isDeviceInTenant = await validateDeviceInTenant.ExecuteAsync(args.TenantId, args.DeviceId, cancellationToken);

        if (!isDeviceInTenant)
        {
            return BadRequest("Device is invalid");
        }
        
        var response = await pollForInstructions.ExecuteAsync(args.TenantId, args.DeviceId, cancellationToken);

        return Ok(response);
    }
}