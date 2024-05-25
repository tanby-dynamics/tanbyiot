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
        var logger = Log.ForContext<GetAllRulesForTenant>();
        
        logger.Information("Getting all rules for tenant {TenantId}", tenantId);

        var rules = await dbContext.Rules
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);

        return rules.Select(x => new RuleDto
        {
            Id = x.Id,
            Name = x.Name,
            Enabled = x.Enabled,
            CreatedAt = x.CreatedAt
        });
    }
}