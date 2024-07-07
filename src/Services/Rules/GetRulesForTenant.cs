using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IGetRulesForTenant
{
    Task<IEnumerable<RuleDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
}

public class GetRulesForTenant(AppDbContext dbContext) : IGetRulesForTenant
{
    public async Task<IEnumerable<RuleDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var rules = await dbContext.Rules
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);

        return rules.Select(RuleDto.FromEntity);
    }
}