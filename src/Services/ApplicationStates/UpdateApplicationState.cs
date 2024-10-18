using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.ApplicationStates;

public interface IUpdateApplicationState
{
    Task<ApplicationStateDto> ExecuteAsync(Guid applicationStateId, UpdateApplicationStateArgs args,
        CancellationToken cancellationToken);
}

public class UpdateApplicationState(AppDbContext dbContext, ISystemClock clock) : IUpdateApplicationState
{
    public async Task<ApplicationStateDto> ExecuteAsync(Guid applicationStateId, UpdateApplicationStateArgs args, CancellationToken cancellationToken)
    {
        var state = await dbContext.ApplicationStates.SingleAsync(x => x.Id == applicationStateId, cancellationToken);

        state.Key = args.Key;
        state.Value = args.Value;
        state.SetAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return ApplicationStateDto.FromEntity(state);
    }
}