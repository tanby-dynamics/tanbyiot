namespace Services.Rules;

public class RuleDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool Enabled { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}