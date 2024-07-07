using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Processing.Conditions;

public class CheckTenantStateCondition(AppDbContext dbContext) : ICheckCondition
{
    public async Task<bool> ExecuteAsync(RuleCondition condition, TenantContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<CheckTenantStateCondition>();
        
        if (condition.Key is null)
        {
            log.Error(
                "Key is null for tenant state condition {RuleConditionId} in tenant {TenantId}",
                condition.Id,
                context.Tenant.Id);
            return false;
        }

        var state = await dbContext.TenantStates.SingleOrDefaultAsync(
            x => x.TenantId == context.Tenant.Id && x.Key == condition.Key,
            cancellationToken);

        return state?.Value == condition.ComparisonValue;
    }
}