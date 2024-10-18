using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Processing.Conditions;

public class CheckApplicationStateCondition(AppDbContext dbContext) : ICheckCondition
{
    public async Task<bool> ExecuteAsync(RuleCondition condition, ApplicationContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<CheckApplicationStateCondition>();
        
        if (condition.Key is null)
        {
            log.Error(
                "Key is null for application state condition {RuleConditionId}",
                condition.Id);
            return false;
        }

        var state = await dbContext.ApplicationStates.SingleOrDefaultAsync(
            x => x.Key == condition.Key,
            cancellationToken);

        return state?.Value == condition.ComparisonValue;
    }
}