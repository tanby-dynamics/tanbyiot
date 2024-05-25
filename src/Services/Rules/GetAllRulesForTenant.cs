using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Rules;

public interface IGetAllRulesForTenant
{
    Task<IEnumerable<RuleDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken);
}

public class GetAllRulesForTenant(AppDbContext dbContext) : IGetAllRulesForTenant
{
    public async Task<IEnumerable<RuleDto>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<GetAllRulesForTenant>();
        log.Information("Getting all rules for tenant {TenantId}", tenantId);

        var rules = await dbContext.Rules
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);

        return rules.Select(RuleDto.FromEntity);
    }
}