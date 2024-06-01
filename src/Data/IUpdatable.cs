namespace Data;

public interface IUpdatable
{
    DateTimeOffset? UpdatedAt { get; set; }
}