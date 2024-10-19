using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Devices;

namespace tanbyiot.Server.Features.Devices;

[ApiController]
[Route("/api/devices")]
public class DevicesController(
    IGetAllDevices getAllDevices, 
    IAddDevice addDevice,
    IGetTelemetryForDevice getTelemetryForDevice,
    IGetInstructionsForDevice getInstructionsForDevice,
    IGetDevice getDevice) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<DeviceDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        var devices = await getAllDevices.ExecuteAsync(cancellationToken);
        
        return Ok(devices);
    }

    [HttpGet("{deviceId:guid}")]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<DevicesController>();
        var (found, device) = await getDevice.ExecuteAsync(deviceId, cancellationToken);

        if (!found)
        {
            log.Warning("Cannot get device {DeviceId}", deviceId);
            return NotFound();
        }

        return Ok(device);
    }

    [HttpPost]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddDevice(AddDeviceRequestDto request, CancellationToken cancellationToken)
    {
        var device = await addDevice.ExecuteAsync(request.Name, request.GroupName, cancellationToken);

        return Ok(device);
    }
 

    [HttpGet("{deviceId:guid}/telemetry")]
    [ProducesResponseType<IEnumerable<TelemetryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTelemetryForDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var results = await getTelemetryForDevice.ExecuteAsync(
            deviceId, 
            100, 
            cancellationToken);

        return Ok(results);
    }

    [HttpGet("{deviceId:guid}/instructions")]
    [ProducesResponseType<IEnumerable<GetInstructionsForDeviceResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetInstructionsForDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var results = await getInstructionsForDevice.ExecuteAsync(
            deviceId, 
            100, 
            cancellationToken);

        return Ok(results);
    }
}