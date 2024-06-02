using Data;

namespace Services.Rules;

public class RuleActionDto
{
    public Guid Id { get; set; }
    public RuleActionType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? SendInstructionType { get; init; }
    public string? SendInstructionValue { get; init; }
    public string? SendInstructionPayload { get; init; }
    public Guid? SendInstructionDeviceId { get; init; }
    public string? SendInstructionDeviceGroups { get; init; }

    public static RuleActionDto FromEntity(RuleAction action)
    {
        return new RuleActionDto
        {
            Id = action.Id,
            Type = action.Type,
            CreatedAt = action.CreatedAt,
            Description = $"Description for {action.Type} action",
            SendInstructionType = action.SendInstructionType,
            SendInstructionValue = action.SendInstructionValue,
            SendInstructionPayload = action.SendInstructionPayload,
            SendInstructionDeviceId = action.SendInstructionDeviceId,
            SendInstructionDeviceGroups = action.SendInstructionDeviceGroups
        };
    }
}