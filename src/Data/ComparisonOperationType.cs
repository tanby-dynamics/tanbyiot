using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComparisonOperationType
{
    Equals,
    NotEquals,
    LessThan,
    GreaterThan,
    IsTrue,
    IsFalse
}