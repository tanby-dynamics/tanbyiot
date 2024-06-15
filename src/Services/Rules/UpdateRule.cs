using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Rules;

public interface IUpdateRule
{
    Task<RuleDto> ExecuteAsync(Guid ruleId, UpdateRuleArgs args, CancellationToken cancellationToken);
}

public class UpdateRule(AppDbContext dbContext) : IUpdateRule
{
    public async Task<RuleDto> ExecuteAsync(Guid ruleId, UpdateRuleArgs args, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<UpdateRule>();
        var rule = await dbContext.Rules
            .SingleAsync(x => x.Id == ruleId, cancellationToken);

        rule.Name = args.Name;
        rule.Enabled = args.Enabled;

        await dbContext.SaveChangesAsync(cancellationToken);

        log.Information("Rule {RuleId} has been updated", ruleId);

        return RuleDto.FromEntity(rule);
    }
}