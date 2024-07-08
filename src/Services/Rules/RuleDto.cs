using Data;

namespace Services.Rules;

public class RuleDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool Enabled { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }

    public static RuleDto FromEntity(Rule rule)
    {
        return new RuleDto
        {
            Id = rule.Id,
            Name = rule.Name,
            Enabled = rule.Enabled,
            CreatedAt = rule.CreatedAt,
            UpdatedAt = rule.UpdatedAt
        };
    }
}