using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationStates;

public interface IDeleteApplicationState
{
    Task ExecuteAsync(Guid applicationStateId, CancellationToken cancellationToken);
}

public class DeleteApplicationState(AppDbContext dbContext) : IDeleteApplicationState
{
    public async Task ExecuteAsync(Guid applicationStateId, CancellationToken cancellationToken)
    {
        await dbContext.ApplicationStates
            .Where(x => x.Id == applicationStateId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}