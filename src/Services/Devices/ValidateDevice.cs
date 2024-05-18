using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IValidateDevice
{
    Task<bool> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken);
}

public class ValidateDevice(AppDbContext dbContext) : IValidateDevice
{
    public async Task<bool> ExecuteAsync(Guid tenantId, Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await dbContext.Devices
            .Include(x=> x.Tenant)
            .SingleOrDefaultAsync(x => x.Id == deviceId, cancellationToken);

        if (device is null)
        {
            return false;
        }

        if (device.Tenant.Id != tenantId)
        {
            return false;
        }

        /*
        if (!device.Tenant.IsActive)
        {
            return false;
        }
        */

        return true;
    }
}