using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Rules;

public interface IUpdateRule
{
    Task<RuleDto> ExecuteAsync(Guid ruleId, UpdateRuleArgs args, CancellationToken cancellationToken);
}

public class UpdateRule(AppDbContext dbContext, ISystemClock clock) : IUpdateRule
{
    public async Task<RuleDto> ExecuteAsync(Guid ruleId, UpdateRuleArgs args, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<UpdateRule>();
        var rule = await dbContext.Rules
            .SingleAsync(x => x.Id == ruleId, cancellationToken);

        rule.Name = args.Name;
        rule.Enabled = args.Enabled;
        rule.UpdatedAt = clock.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        log.Information("Rule {RuleId} has been updated", ruleId);

        return RuleDto.FromEntity(rule);
    }
}