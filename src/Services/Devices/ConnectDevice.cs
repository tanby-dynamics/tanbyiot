using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.Devices;

public interface IConnectDevice
{
    Task ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class ConnectDevice(AppDbContext dbContext, ISystemClock clock) : IConnectDevice
{
    public async Task ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices.SingleAsync(
            x => x.TenantId == tenantId && x.Id == deviceId, 
            cancellationToken);

        device.LastConnected = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}