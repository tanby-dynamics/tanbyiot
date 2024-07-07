using Data;

namespace Services.TenantStates;

public class TenantStateDto
{
    public Guid Id { get; init; }
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public DateTimeOffset SetAt { get; init; }
    
    public static TenantStateDto FromEntity(TenantState entity)
    {
        return new TenantStateDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Value = entity.Value,
            SetAt = entity.SetAt
        };
    }
}