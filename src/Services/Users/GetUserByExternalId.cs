using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Users;

public interface IGetUserByExternalId
{
    Task<(bool found, UserDto? user)> ExecuteAsync(string? externalId, CancellationToken cancellationToken);
}

public class GetUserByExternalId(AppDbContext dbContext) : IGetUserByExternalId
{
    public async Task<(bool found, UserDto? user)> ExecuteAsync(string? externalId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.Tenants)
            .Include(x => x.CurrentTenant)
            .SingleOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);

        if (user is not null && user.CurrentTenant is null && user.Tenants.Count > 0)
        {
            user.CurrentTenant = user.Tenants.First();
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return user is null 
            ? (false, null) 
            : (true, UserDto.FromEntity(user));
    }
}