using Data;
using Microsoft.EntityFrameworkCore;
using Services.Rules.Actions;
using Services.Rules.Conditions;

namespace Services.Rules;

public interface IGetRuleDetail
{
    Task<RuleDetailDto> ExecuteAsync(Guid ruleId, CancellationToken cancellationToken);
}

public class GetRuleDetail(AppDbContext dbContext) : IGetRuleDetail
{
    public async Task<RuleDetailDto> ExecuteAsync(Guid ruleId, CancellationToken cancellationToken)
    {
        var rule = await dbContext.Rules
            .Include(x => x.Conditions)
            .Include(x => x.Actions)
            .SingleAsync(x => x.Id == ruleId, cancellationToken);

        return new RuleDetailDto
        {
            Id = rule.Id,
            Name = rule.Name,
            Enabled = rule.Enabled,
            CreatedAt = rule.CreatedAt,
            Conditions = rule.Conditions.Select(RuleConditionDto.FromEntity),
            Actions = rule.Actions.Select(RuleActionDto.FromEntity)
        };
    }
}