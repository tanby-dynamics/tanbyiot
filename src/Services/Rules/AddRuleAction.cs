using Data;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Rules;

public interface IAddRuleAction
{
    Task<RuleActionDto> ExecuteAsync(Guid ruleId, RuleActionType type, CancellationToken cancellationToken);
}

public class AddRuleAction(AppDbContext dbContext, ISystemClock clock) : IAddRuleAction
{
    public async Task<RuleActionDto> ExecuteAsync(Guid ruleId, RuleActionType type, CancellationToken cancellationToken)
    {
        var result = await dbContext.RuleActions.AddAsync(new RuleAction
        {
            RuleId = ruleId,
            Type = type,
            CreatedAt = clock.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        var log = Log.ForContext<AddRuleAction>();
        log.Information("Added action {RuleActionId} for rule {RuleId}", result.Entity.Id, ruleId);

        return RuleActionDto.FromEntity(result.Entity);
    }
}