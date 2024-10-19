using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.ApplicationStates;

public interface IDeleteApplicationState
{
    Task ExecuteAsync(Guid applicationStateId, CancellationToken cancellationToken);
}

public class DeleteApplicationState(AppDbContext dbContext, ISystemClock clock) : IDeleteApplicationState
{
    public async Task ExecuteAsync(Guid applicationStateId, CancellationToken cancellationToken)
    {
        var state = await dbContext.ApplicationStates
            .SingleAsync(x => x.Id == applicationStateId, cancellationToken);

        state.DeletedAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}