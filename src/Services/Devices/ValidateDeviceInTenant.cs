using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IValidateDeviceInTenant
{
    Task<bool> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class ValidateDeviceInTenant(AppDbContext dbContext) : IValidateDeviceInTenant
{
    public async Task<bool> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        return await dbContext.Devices
            .AnyAsync(x => x.TenantId == tenantId && x.Id == deviceId, cancellationToken);
    }
}