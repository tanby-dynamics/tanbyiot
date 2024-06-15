using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Users;

public interface IValidateTenantForUser
{
    Task<bool> ExecuteAsync(Guid tenantId, string externalId, CancellationToken cancellationToken);
}

public class ValidateTenantForUser(AppDbContext dbContext) : IValidateTenantForUser
{
    public async Task<bool> ExecuteAsync(Guid tenantId, string externalId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.Tenants)
            .SingleOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);

        return user is not null && user.Tenants.Any(x => x.Id == tenantId);
    }
}