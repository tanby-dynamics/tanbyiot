using Data;
using Serilog;

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
        
        Log.Information("Added device {DeviceId} for {TenantId}", result.Entity.Id, tenantId);
        
        return DeviceDto.FromEntity(result.Entity);
    }
}