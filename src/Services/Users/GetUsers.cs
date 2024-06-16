using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Users;

public interface IGetUsers
{
    Task<IEnumerable<UserDto>> ExecuteAsync(CancellationToken cancellationToken);
}

public class GetUsers(AppDbContext dbContext) : IGetUsers
{
    public async Task<IEnumerable<UserDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var users = await dbContext.Users
            .Include(x => x.Tenants)
            .Include(x => x.CurrentTenant)
            .ToListAsync(cancellationToken);

        return users.Select(UserDto.FromEntity);
    }
}