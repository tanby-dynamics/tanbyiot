using Data;

namespace Services.Rules.Actions;

public class RuleActionDto
{
    public Guid Id { get; set; }
    public RuleActionType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? SendInstructionType { get; init; }
    public string? SendInstructionValue { get; init; }
    public string? Key { get; init; }
    public string? Payload { get; init; }
    public Guid? SendInstructionDeviceId { get; init; }
    public string? SendInstructionDeviceGroups { get; init; }
    public RuleActionSendInstructionTargetDeviceType? SendInstructionTargetDeviceType { get; init; }
    public string? SendEmailSenderEmail { get; init; }
    public string? SendEmailSenderName { get; init; }
    public string? SendEmailToEmail { get; init; }
    public string? SendEmailBody { get; init; }
    public string? SendEmailSubject { get; init; }

    public static RuleActionDto FromEntity(RuleAction action)
    {
        var sendInstructionDeviceTypeDescription = action.SendInstructionTargetDeviceType switch
        {
            RuleActionSendInstructionTargetDeviceType.DeviceGroups =>
                $"device groups \"{action.SendInstructionDeviceGroups}\"",
            RuleActionSendInstructionTargetDeviceType.SingleDevice => "device",
            _ => $"Unknown target device type {action.SendInstructionTargetDeviceType}"
        };
        var description = action.Type switch
        {
            RuleActionType.SendInstruction => $"Send \"{action.SendInstructionType}\" instruction to {sendInstructionDeviceTypeDescription}",
            RuleActionType.SetState => $"Set tenant state key \"{action.Key}\" to \"{action.Payload}\"",
            RuleActionType.SendEmail => $"Send email with subject \"{action.SendEmailSubject}\"",
            _ => $"Unknown rule action type {action.Type}"
        };
        
        return new RuleActionDto
        {
            Id = action.Id,
            Type = action.Type,
            CreatedAt = action.CreatedAt,
            Description = description,
            SendInstructionType = action.SendInstructionType,
            SendInstructionValue = action.SendInstructionValue,
            Payload = action.Payload,
            SendInstructionDeviceId = action.SendInstructionDeviceId,
            SendInstructionDeviceGroups = action.SendInstructionDeviceGroups,
            SendInstructionTargetDeviceType = action.SendInstructionTargetDeviceType,
            Key = action.Key,
            SendEmailSenderEmail = action.SendEmailSenderEmail,
            SendEmailSenderName = action.SendEmailSenderName,
            SendEmailToEmail = action.SendEmailToEmail,
            SendEmailSubject = action.SendEmailSubject,
            SendEmailBody = action.SendEmailBody
        };
    }
}