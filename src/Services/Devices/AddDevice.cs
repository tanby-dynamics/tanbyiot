using Data;

namespace Services.Devices;

public interface IAddDevice
{
    Task<DeviceDto> ExecuteAsync(Guid tenantId, string name, string groupName, CancellationToken cancellationToken);
}

public class AddDevice(AppDbContext dbContext) : IAddDevice
{
    public async Task<DeviceDto> ExecuteAsync(Guid tenantId, string name, string groupName, CancellationToken cancellationToken)
    {
        var result = await dbContext.Devices.AddAsync(new Device
        {
            TenantId = tenantId,
            Name = name,
            GroupName = groupName
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return DeviceDto.FromEntity(result.Entity);
    }
}