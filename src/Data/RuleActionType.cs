using System.Text.Json.Serialization;

namespace Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleActionType
{
    SendInstruction,
    TriggerWebhook,
    SendEmail,
    SendSms,
    SetState
}