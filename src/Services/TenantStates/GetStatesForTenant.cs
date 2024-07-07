using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.TenantStates;

public interface IGetStatesForTenant
{
    Task<IEnumerable<TenantStateDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
}

public class GetStatesForTenant(AppDbContext dbContext) : IGetStatesForTenant
{
    public async Task<IEnumerable<TenantStateDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var states = await dbContext.TenantStates
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);

        return states.Select(TenantStateDto.FromEntity);
    }
}