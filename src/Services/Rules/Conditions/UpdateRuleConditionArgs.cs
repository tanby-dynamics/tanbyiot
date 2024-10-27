﻿using Data;

namespace Services.Rules.Conditions;

public class UpdateRuleConditionArgs
{
    public Guid RuleConditionId { get; init; }
    public string? ComparisonValue { get; init; }
    public ComparisonOperationType? ApplicationValueStateComparisonOperationType { get; init; }
    public string? PayloadPath { get; init; }
    public RuleConditionConversionType? Conversion { get; init; }
    public string? Key { get; init; }
    public TelemetryTypeMatchingType TelemetryTypeMatchingType { get; init; }
}