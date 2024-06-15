using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Users;

public interface ISetCurrentTenantForUser
{
    Task ExecuteAsync(UserDto user, Guid tenantId, CancellationToken cancellationToken);
}

public class SetCurrentTenantForUser(AppDbContext dbContext) : ISetCurrentTenantForUser
{
    public async Task ExecuteAsync(UserDto user, Guid tenantId, CancellationToken cancellationToken)
    {
        var userEntity = await dbContext.Users
            .Include(x => x.Tenants)
            .SingleAsync(x => x.Id == user.Id, cancellationToken);

        userEntity.CurrentTenant = userEntity.Tenants.Single(x => x.Id == tenantId);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}