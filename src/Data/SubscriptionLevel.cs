using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionLevel
{
    Developer,
    Startup,
    Business,
    Industry
}