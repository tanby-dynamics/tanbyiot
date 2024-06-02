using Data;

namespace Services.Rules;

public class UpdateRuleActionArgs
{
    public string? SendInstructionType { get; init; }
    public string? SendInstructionValue { get; init; }
    public string? SendInstructionPayload { get; init; }
    public Guid? SendInstructionDeviceId { get; init; }
    public string? SendInstructionDeviceGroups { get; init; }
    public RuleActionSendInstructionTargetDeviceType? SendInstructionTargetDeviceType { get; init; }
}