using Data;

namespace Services.Rules;

public class RuleConditionDto
{
    public Guid Id { get; set; }
    public RuleConditionType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;

    public static RuleConditionDto FromEntity(RuleCondition condition)
    {
        return new RuleConditionDto
        {
            Id = condition.Id,
            Type = condition.Type,
            CreatedAt = condition.CreatedAt,
            Description = $"Description for {condition.Type} condition"
        };
    }
}