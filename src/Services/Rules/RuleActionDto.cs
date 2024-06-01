using Data;

namespace Services.Rules;

public class RuleActionDto
{
    public Guid Id { get; set; }
    public RuleActionType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;

    public static RuleActionDto FromEntity(RuleAction action)
    {
        return new RuleActionDto
        {
            Id = action.Id,
            Type = action.Type,
            CreatedAt = action.CreatedAt,
            Description = $"Description for {action.Type} action"
        };
    }
}