﻿using Data;

namespace Services.Rules;

public class UpdateRuleConditionArgs
{
    public Guid RuleConditionId { get; init; }
    public string? ComparisonValue { get; init; }
    public RuleConditionComparisonOperationType? ComparisonOperation { get; init; }
    public string? PayloadPath { get; init; }
    public RuleConditionConversionType? Conversion { get; init; }
}