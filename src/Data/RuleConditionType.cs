using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleConditionType
{
    TelemetryTypes,
    Value,
    Payload,
    DeviceId,
    Group,
    State
}