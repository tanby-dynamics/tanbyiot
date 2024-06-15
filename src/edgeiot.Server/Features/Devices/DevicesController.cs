﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Devices;

namespace edgeiot.Server.Features.Devices;

[ApiController]
[Route("/api/devices")]
[Authorize]
public class DevicesController(
    IGetAllDevicesForTenant getAllDevicesForTenant, 
    IAddDevice addDevice,
    IValidateDevice validateDevice,
    IConnectDevice connectDevice,
    IGetTelemetryForDevice getTelemetryForDevice,
    IGetInstructionsForDevice getInstructionsForDevice,
    IGetDevice getDevice) : ControllerBase
{
    public static Guid TenantId => Guid.Parse("de37f1e6-70a1-4c69-bdbc-317ff86b5267"); // Hard-coded to our single initial test tenant
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<DeviceDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDevices(CancellationToken cancellationToken)
    {
        var devices = await getAllDevicesForTenant.ExecuteAsync(
            TenantId,
            cancellationToken);
        
        return Ok(devices);
    }

    [HttpGet("{deviceId:guid}")]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await getDevice.ExecuteAsync(TenantId, deviceId, cancellationToken);

        return Ok(device);
    }

    [HttpPost]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddDevice(AddDeviceRequestDto request, CancellationToken cancellationToken)
    {
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

    [HttpGet("{deviceId:guid}/telemetry")]
    [ProducesResponseType<IEnumerable<TelemetryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTelemetryForDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var isDeviceValidated = await validateDevice.ExecuteAsync(TenantId, deviceId, cancellationToken);

        if (!isDeviceValidated)
        {
            return BadRequest("Device is invalid");
        }
        
        var results = await getTelemetryForDevice.ExecuteAsync(deviceId, 100, cancellationToken);

        return Ok(results);
    }

    [HttpGet("{deviceId:guid}/instructions")]
    [ProducesResponseType<IEnumerable<GetInstructionsForDeviceResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetInstructionsForDevice(Guid deviceId, CancellationToken cancellationToken)
    {
        var isDeviceValidated = await validateDevice.ExecuteAsync(TenantId, deviceId, cancellationToken);

        if (!isDeviceValidated)
        {
            return BadRequest("Device is invalid");
        }
        
        var results = await getInstructionsForDevice.ExecuteAsync(deviceId, 100, cancellationToken);

        return Ok(results);
    }
}