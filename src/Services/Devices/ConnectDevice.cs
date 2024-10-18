using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Devices;

public interface IConnectDevice
{
    Task ExecuteAsync(Guid deviceId, CancellationToken cancellationToken);
}

public class ConnectDevice(AppDbContext dbContext, ISystemClock clock) : IConnectDevice
{
    public async Task ExecuteAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices.SingleOrDefaultAsync(
            x => x.Id == deviceId, 
            cancellationToken);

        if (device is null)
        {
            Log.Warning("Device {DeviceId} not found, cannot connect", deviceId);
            return;
        }

        device.LastConnected = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        
        Log.Information("Connected device {DeviceId}", deviceId);
    }
}