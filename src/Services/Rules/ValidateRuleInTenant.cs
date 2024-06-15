using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IValidateRuleInTenant
{
    Task<bool> ExecuteAsync(Guid tenantId, Guid ruleId, CancellationToken cancellationToken);
}

public class ValidateRuleInTenant(AppDbContext dbContext) : IValidateRuleInTenant
{
    public async Task<bool> ExecuteAsync(Guid tenantId, Guid ruleId, CancellationToken cancellationToken)
    {
        return await dbContext.Rules.AnyAsync(x => x.TenantId == tenantId && x.Id == ruleId, cancellationToken);
    }
}