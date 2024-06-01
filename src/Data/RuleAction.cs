namespace Data;

public class RuleAction : ISoftDelete
{
    public Guid Id { get; init; }
    public Guid RuleId { get; init; }
    public RuleActionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public virtual Rule Rule { get; init; }
}