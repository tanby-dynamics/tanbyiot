using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.TenantStates;

public interface IValidateTenantStateInTenant
{
    Task<bool> ExecuteAsync(Guid tenantId, Guid tenantStateId, CancellationToken cancellationToken);
}

public class ValidateTenantStateInTenant(AppDbContext dbContext) : IValidateTenantStateInTenant
{
    public async Task<bool> ExecuteAsync(Guid tenantId, Guid tenantStateId, CancellationToken cancellationToken)
    {
        return await dbContext.TenantStates
            .AnyAsync(x => x.TenantId == tenantId && x.Id == tenantStateId, cancellationToken);
    }
}