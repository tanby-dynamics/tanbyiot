using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Rules;

public interface IGetRulesForTenant
{
    Task<IEnumerable<RuleDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
}

public class GetRulesForTenant(AppDbContext dbContext) : IGetRulesForTenant
{
    public async Task<IEnumerable<RuleDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<GetRulesForTenant>();
        log.Information("Getting all rules for tenant {TenantId}", tenantId);

        var rules = await dbContext.Rules
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);

        return rules.Select(RuleDto.FromEntity);
    }
}