using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetDevice
{
    Task<DeviceDto> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class GetDevice(AppDbContext dbContext) : IGetDevice
{
    public async Task<DeviceDto> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var device =
            await dbContext.Devices.SingleAsync(x => x.TenantId == tenantId && x.Id == deviceId, cancellationToken);

        return DeviceDto.FromEntity(device);
    }
}