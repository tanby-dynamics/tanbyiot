using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleConditionType
{
    TelemetryType,
    Value,
    Payload,
    DeviceId,
    Group
}