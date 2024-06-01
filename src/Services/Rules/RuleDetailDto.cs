namespace Services.Rules;

public class RuleDetailDto : RuleDto
{
    public IEnumerable<RuleConditionDto> Conditions { get; set; } = Array.Empty<RuleConditionDto>();
    public IEnumerable<RuleActionDto> Actions { get; set; } = Array.Empty<RuleActionDto>();
}