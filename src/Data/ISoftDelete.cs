namespace Data;

public interface ISoftDelete
{
    DateTimeOffset? DeletedAt { get; set; }
}