namespace Services.Users;

public interface IGetCurrentTenantIdForUser
{
    Task<(bool found, Guid tenantId)> ExecuteAsync(string? externalId, CancellationToken cancellationToken);
}

public class GetCurrentTenantIdForUser(IGetUserByExternalId getUserByExternalId) : IGetCurrentTenantIdForUser
{
    public async Task<(bool found, Guid tenantId)> ExecuteAsync(string? externalId, CancellationToken cancellationToken)
    {
        var (found, user) = await getUserByExternalId.ExecuteAsync(externalId, cancellationToken);

        if (!found || user?.CurrentTenant is null)
        {
            return (false, Guid.Empty);
        }

        return (true, user.CurrentTenant.Id);
    }
}