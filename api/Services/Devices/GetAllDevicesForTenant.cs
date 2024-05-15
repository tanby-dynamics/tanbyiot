using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetAllDevicesForTenant
{
    Task<IEnumerable<DeviceDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
}

public class GetAllDevicesForTenant(AppDbContext dbContext) : IGetAllDevicesForTenant
{
    public async Task<IEnumerable<DeviceDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var devices = await dbContext.Devices
            .Where(x => x.TenantId == tenantId)
            .Select(x => new DeviceDto
            {
                Id = x.Id,
                Name = x.Name,
                GroupName = x.GroupName,
                LastConnected = x.LastConnected
            })
            .ToListAsync(cancellationToken);

        return devices;
    }
}