using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleConditionConversionType
{
    Number,
    String,
    Boolean
}