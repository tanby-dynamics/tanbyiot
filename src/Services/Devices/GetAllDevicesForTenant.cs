using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetAllDevices
{
    Task<IEnumerable<DeviceDto>> ExecuteAsync(CancellationToken cancellationToken);
}

public class GetAllDevices(AppDbContext dbContext) : IGetAllDevices
{
    public async Task<IEnumerable<DeviceDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var devices = await dbContext.Devices
            .ToListAsync(cancellationToken);

        return devices.Select(DeviceDto.FromEntity);
    }
}