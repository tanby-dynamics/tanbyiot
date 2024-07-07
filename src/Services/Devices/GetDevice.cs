using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetDevice
{
    Task<(bool found, DeviceDto? device)> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class GetDevice(AppDbContext dbContext) : IGetDevice
{
    public async Task<(bool found, DeviceDto? device)> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var device =
            await dbContext.Devices.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.Id == deviceId, cancellationToken);

        return device is null 
            ? (false, null) 
            : (true, DeviceDto.FromEntity(device));
    }
}