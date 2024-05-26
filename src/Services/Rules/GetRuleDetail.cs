using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IGetRuleDetail
{
    Task<RuleDetailDto> ExecuteAsync(Guid tenantId, Guid ruleId, CancellationToken cancellationToken);
}

public class GetRuleDetail(AppDbContext dbContext) : IGetRuleDetail
{
    public async Task<RuleDetailDto> ExecuteAsync(Guid tenantId, Guid ruleId, CancellationToken cancellationToken)
    {
        var rule = await dbContext.Rules
            .SingleAsync(x => x.TenantId == tenantId && x.Id == ruleId, cancellationToken);

        return new RuleDetailDto
        {
            Id = rule.Id,
            Name = rule.Name,
            Enabled = rule.Enabled,
            CreatedAt = rule.CreatedAt,
            Conditions = Array.Empty<RuleConditionDto>(),
            Actions = Array.Empty<RuleActionDto>()
        };
    }
}

public class RuleDetailDto : RuleDto
{
    public IEnumerable<RuleConditionDto> Conditions { get; set; } = Array.Empty<RuleConditionDto>();
    public IEnumerable<RuleActionDto> Actions { get; set; } = Array.Empty<RuleActionDto>();
}

public class RuleConditionDto
{
    public Guid Id { get; set; }
}

public class RuleActionDto
{
    public Guid Id { get; set; }
}