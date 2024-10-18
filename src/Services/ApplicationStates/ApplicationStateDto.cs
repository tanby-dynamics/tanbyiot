using Data;

namespace Services.ApplicationStates;

public class ApplicationStateDto
{
    public Guid Id { get; init; }
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public DateTimeOffset SetAt { get; init; }
    
    public static ApplicationStateDto FromEntity(ApplicationState entity)
    {
        return new ApplicationStateDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Value = entity.Value,
            SetAt = entity.SetAt
        };
    }
}