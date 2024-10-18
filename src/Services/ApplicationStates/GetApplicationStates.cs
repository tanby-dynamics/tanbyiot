using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationStates;

public interface IGetApplicationStates
{
    Task<IEnumerable<ApplicationStateDto>> ExecuteAsync(CancellationToken cancellationToken);
}

public class GetApplicationApplicationStates(AppDbContext dbContext) : IGetApplicationStates
{
    public async Task<IEnumerable<ApplicationStateDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var states = await dbContext.ApplicationStates
            .ToListAsync(cancellationToken);

        return states.Select(ApplicationStateDto.FromEntity);
    }
}