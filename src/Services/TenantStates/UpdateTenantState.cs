using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.TenantStates;

public interface IUpdateTenantState
{
    Task<TenantStateDto> ExecuteAsync(Guid tenantRuleId, UpdateTenantStateArgs args,
        CancellationToken cancellationToken);
}

public class UpdateTenantState(AppDbContext dbContext, ISystemClock clock) : IUpdateTenantState
{
    public async Task<TenantStateDto> ExecuteAsync(Guid tenantRuleId, UpdateTenantStateArgs args, CancellationToken cancellationToken)
    {
        var state = await dbContext.TenantStates.SingleAsync(x => x.Id == tenantRuleId, cancellationToken);

        state.Key = args.Key;
        state.Value = args.Value;
        state.SetAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return TenantStateDto.FromEntity(state);
    }
}