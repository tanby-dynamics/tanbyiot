using Data;

namespace Services.Rules.Actions;

public class UpdateRuleActionArgs
{
    public string? SendInstructionType { get; init; }
    public string? SendInstructionValue { get; init; }
    public string? Payload { get; init; }
    public string? Key { get; init; }
    public Guid? SendInstructionDeviceId { get; init; }
    public string? SendInstructionDeviceGroups { get; init; }
    public RuleActionSendInstructionTargetDeviceType? SendInstructionTargetDeviceType { get; init; }
}