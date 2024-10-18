namespace Services.ApplicationStates;

// ReSharper disable once ClassNeverInstantiated.Global
public class UpdateApplicationStateArgs
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}