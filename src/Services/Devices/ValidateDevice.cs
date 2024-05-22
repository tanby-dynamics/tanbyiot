﻿using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Devices;

public interface IValidateDevice
{
    Task<bool> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class ValidateDevice(AppDbContext dbContext) : IValidateDevice
{
    public async Task<bool> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices
            .Include(x=> x.Tenant)
            .SingleOrDefaultAsync(x => x.Id == deviceId, cancellationToken);

        if (device is null)
        {
            Log.Warning("Cannot validate device, {DeviceId} not found", deviceId);
            return false;
        }

        if (device.Tenant.Id != tenantId)
        {
            Log.Warning("Cannot validate device {DeviceId}, does not belong to {TenantId}", deviceId, tenantId);
            return false;
        }

        /*
        if (!device.Tenant.IsActive)
        {
            return false;
        }
        */

        return true;
    }
}