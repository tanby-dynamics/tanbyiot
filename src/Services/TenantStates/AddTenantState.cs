using Data;
using Microsoft.Extensions.Internal;

namespace Services.TenantStates;

public interface IAddTenantState
{
    Task<TenantStateDto> ExecuteAsync(Guid tenantId, AddTenantStateArgs args, CancellationToken cancellationToken);
}

public class AddTenantState(AppDbContext dbContext, ISystemClock clock) : IAddTenantState
{
    public async Task<TenantStateDto> ExecuteAsync(Guid tenantId, AddTenantStateArgs args, CancellationToken cancellationToken)
    {
        var result = await dbContext.TenantStates.AddAsync(new TenantState
        {
            TenantId = tenantId,
            Key = args.Key,
            Value = args.Value,
            SetAt = clock.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return TenantStateDto.FromEntity(result.Entity);
    }
}