using System.ComponentModel.DataAnnotations;

namespace Data;

public class ApplicationState
{
    public Guid Id { get; init; }
    [MaxLength(128)] public string Key { get; set; } = string.Empty;
    [MaxLength(4000)] public string Value { get; set; } = string.Empty;
    public DateTimeOffset SetAt { get; set; }
    public DateTimeOffset? DeletedAt {get; set; }
}