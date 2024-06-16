using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Users;

public interface ISetUserEmail
{
    Task<UserDto> ExecuteAsync(Guid userId, string email, CancellationToken cancellationToken);
}

public class SetUserEmail(AppDbContext dbContext) : ISetUserEmail
{
    public async Task<UserDto> ExecuteAsync(Guid userId, string email, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(x => x.Tenants)
            .Include(x => x.CurrentTenant)
            .SingleAsync(x => x.Id == userId, cancellationToken);

        user.Email = email;

        await dbContext.SaveChangesAsync(cancellationToken);

        return UserDto.FromEntity(user);
    }
}