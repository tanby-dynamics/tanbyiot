using Data;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Users;

public interface IAddUser
{
    Task<UserDto> ExecuteAsync(string externalId, CancellationToken cancellationToken);
}

public class AddUser(AppDbContext dbContext, ISystemClock clock) : IAddUser
{
    public async Task<UserDto> ExecuteAsync(string externalId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<AddUser>();
        var result = await dbContext.Users.AddAsync(new User
        {
            ExternalId = externalId,
            CreatedAt = clock.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        log.Information("Added user {UserId}", result.Entity.Id);

        return UserDto.FromEntity(result.Entity);
    }
}