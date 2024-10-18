using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IGetRules
{
    Task<IEnumerable<RuleDto>> ExecuteAsync(CancellationToken cancellationToken);
}

public class GetRules(AppDbContext dbContext) : IGetRules
{
    public async Task<IEnumerable<RuleDto>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var rules = await dbContext.Rules
            .ToListAsync(cancellationToken);

        return rules.Select(RuleDto.FromEntity);
    }
}