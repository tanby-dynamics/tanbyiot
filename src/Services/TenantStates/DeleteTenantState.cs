using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.TenantStates;

public interface IDeleteTenantState
{
    Task ExecuteAsync(Guid tenantStateId, CancellationToken cancellationToken);
}

public class DeleteTenantState(AppDbContext dbContext) : IDeleteTenantState
{
    public async Task ExecuteAsync(Guid tenantStateId, CancellationToken cancellationToken)
    {
        await dbContext.TenantStates
            .Where(x => x.Id == tenantStateId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}