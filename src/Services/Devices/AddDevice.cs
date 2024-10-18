using Data;
using Serilog;

namespace Services.Devices;

public interface IAddDevice
{
    Task<DeviceDto> ExecuteAsync(string name, string groupName, CancellationToken cancellationToken);
}

public class AddDevice(AppDbContext dbContext) : IAddDevice
{
    public async Task<DeviceDto> ExecuteAsync(string name, string groupName, CancellationToken cancellationToken)
    {
        var result = await dbContext.Devices.AddAsync(new Device
        {
            Name = name,
            GroupName = groupName
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        Log.Information("Added device {DeviceId}", result.Entity.Id);
        
        return DeviceDto.FromEntity(result.Entity);
    }
}