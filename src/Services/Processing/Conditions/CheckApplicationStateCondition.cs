using Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Services.Processing.Conditions;

public class CheckApplicationStateCondition(AppDbContext dbContext) : ICheckCondition
{
    public async Task<bool> ExecuteAsync(RuleCondition condition, ApplicationContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<CheckApplicationStateCondition>();
        
        if (condition.ApplicationStateMatchingKey is null)
        {
            log.Error(
                "Key is null for application state condition {RuleConditionId}",
                condition.Id);
            return false;
        }

        var state = await dbContext.ApplicationStates.SingleOrDefaultAsync(
            x => x.Key == condition.ApplicationStateMatchingKey,
            cancellationToken);

        if (state is null)
        {
            log.Error(
                "Application state {Key} not found for application state condition {RuleConditionId}",
                condition.ApplicationStateMatchingKey,
                condition.Id);
            return false;
        }

        if (!CheckConditionForApplicationStateValue.Execute(condition, state))
        {
            return false;
        }

        return true;
    }
}