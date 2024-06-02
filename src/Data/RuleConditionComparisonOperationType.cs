using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleConditionComparisonOperationType
{
    Equals,
    NotEquals,
    LessThan
}