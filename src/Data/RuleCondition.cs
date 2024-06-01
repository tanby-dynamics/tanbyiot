namespace Data;

public class RuleCondition : ISoftDelete
{
    public Guid Id { get; init; }
    public Guid RuleId { get; init; }
    public RuleConditionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public virtual Rule Rule { get; init; }
}