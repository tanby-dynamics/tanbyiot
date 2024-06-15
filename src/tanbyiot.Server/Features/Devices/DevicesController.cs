using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Devices;
using Services.Users;

namespace tanbyiot.Server.Features.Devices;

[ApiController]
[Route("/api/tenants/{tenantId:guid}/devices")]
[Authorize]
public class DevicesController(
    IGetAllDevicesForTenant getAllDevicesForTenant, 
    IAddDevice addDevice,
    IGetTelemetryForDevice getTelemetryForDevice,
    IGetInstructionsForDevice getInstructionsForDevice,
    IGetDevice getDevice,
    IGetCurrentTenantIdForUser getCurrentTenantIdForUser,
    IValidateTenantForUser validateTenantForUser,
    IValidateDeviceInTenant validateDeviceInTenant) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<DeviceDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDevices(Guid tenantId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<DevicesController>();
        var isTenantValidForUser = await validateTenantForUser.ExecuteAsync(tenantId, HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (!isTenantValidForUser)
        {
            log.Warning("Tenant {TenantId} not valid for user {ExternalId}",
                tenantId,
                HttpContext.User.Identity?.Name!,
                cancellationToken);
            return NotFound();
        }

        var devices = await getAllDevicesForTenant.ExecuteAsync(tenantId, cancellationToken);
        
        return Ok(devices);
    }

    [HttpGet("{deviceId:guid}")]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDevice(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<DevicesController>();
        var isTenantValidForUser = await validateTenantForUser.ExecuteAsync(tenantId, HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (!isTenantValidForUser)
        {
            log.Warning("Tenant {TenantId} not valid for user {ExternalId}",
                tenantId,
                HttpContext.User.Identity?.Name!,
                cancellationToken);
            return NotFound();
        }

        var (found, device) = await getDevice.ExecuteAsync(tenantId, deviceId, cancellationToken);

        if (!found)
        {
            return NotFound();
        }

        return Ok(device);
    }

    [HttpPost]
    [ProducesResponseType<DeviceDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddDevice(Guid tenantId, AddDeviceRequestDto request, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<DevicesController>();
        var isTenantValidForUser = await validateTenantForUser.ExecuteAsync(tenantId, HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (!isTenantValidForUser)
        {
            log.Warning("Tenant {TenantId} not valid for user {ExternalId}",
                tenantId,
                HttpContext.User.Identity?.Name!,
                cancellationToken);
            return NotFound();
        }

        var device = await addDevice.ExecuteAsync(tenantId, request.Name, request.GroupName, cancellationToken);

        return Ok(device);
    }
 

    [HttpGet("{deviceId:guid}/telemetry")]
    [ProducesResponseType<IEnumerable<TelemetryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTelemetryForDevice(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<DevicesController>();
        var isTenantValidForUser = await validateTenantForUser.ExecuteAsync(tenantId, HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (!isTenantValidForUser)
        {
            log.Warning("Tenant {TenantId} not valid for user {ExternalId}",
                tenantId,
                HttpContext.User.Identity?.Name!,
                cancellationToken);
            return NotFound();
        }

        var deviceInTenant = await validateDeviceInTenant.ExecuteAsync(tenantId, deviceId, cancellationToken);

        if (!deviceInTenant)
        {
            log.Warning("Device {DeviceId} not in tenant {TenantId}", deviceId, tenantId);
            return NotFound();
        }
        
        var results = await getTelemetryForDevice.ExecuteAsync(
            deviceId, 
            100, 
            cancellationToken);

        return Ok(results);
    }

    [HttpGet("{deviceId:guid}/instructions")]
    [ProducesResponseType<IEnumerable<GetInstructionsForDeviceResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetInstructionsForDevice(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<DevicesController>();
        var tenantValidForUser = await validateTenantForUser.ExecuteAsync(
            tenantId,
            HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (!tenantValidForUser)
        {
            log.Warning("Tenant {TenantId} not valid for user {ExternalId}",
                tenantId,
                HttpContext.User.Identity?.Name!,
                cancellationToken);
            return NotFound();
        }

        var deviceInTenant = await validateDeviceInTenant.ExecuteAsync(tenantId, deviceId, cancellationToken);

        if (!deviceInTenant)
        {
            log.Warning("Device {DeviceId} not in tenant {TenantId}", deviceId, tenantId);
            return NotFound();
        }
        
        var results = await getInstructionsForDevice.ExecuteAsync(
            deviceId, 
            100, 
            cancellationToken);

        return Ok(results);
    }
}