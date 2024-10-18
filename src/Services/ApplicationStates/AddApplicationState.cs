using Data;
using Microsoft.Extensions.Internal;

namespace Services.ApplicationStates;

public interface IAddApplicationState
{
    Task<ApplicationStateDto> ExecuteAsync(AddApplicationStateArgs args, CancellationToken cancellationToken);
}

public class AddApplicationState(AppDbContext dbContext, ISystemClock clock) : IAddApplicationState
{
    public async Task<ApplicationStateDto> ExecuteAsync(AddApplicationStateArgs args, CancellationToken cancellationToken)
    {
        var result = await dbContext.ApplicationStates.AddAsync(new ApplicationState
        {
            Key = args.Key,
            Value = args.Value,
            SetAt = clock.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return ApplicationStateDto.FromEntity(result.Entity);
    }
}