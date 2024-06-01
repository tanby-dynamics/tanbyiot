using Data;

namespace edgeiot.Server.Features.Rules;

public class UpdateRuleConditionRequestDto
{
    public string? ComparisonValue { get; set; }
    public RuleConditionComparisonOperationType? ComparisonOperation { get; set; }
    public string? PayloadPath { get; set; }
    public RuleConditionConversionType? Conversion { get; set; }
}