using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Devices;

public interface IConnectDevice
{
    Task ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class ConnectDevice(AppDbContext dbContext, ISystemClock clock) : IConnectDevice
{
    public async Task ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices.SingleOrDefaultAsync(
            x => x.TenantId == tenantId && x.Id == deviceId, 
            cancellationToken);

        if (device is null)
        {
            Log.Warning("Device with {TenantId} and {DeviceId} not found, cannot connect", tenantId, device);
            return;
        }

        device.LastConnected = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        
        Log.Information("Connected device with {TenantId} and {DeviceId}", tenantId, deviceId);
    }
}