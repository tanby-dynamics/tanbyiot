using Data;

namespace Services.Processing.Conditions;

public static class CheckConditionForTelemetryTypes
{
    public static bool Execute(RuleCondition condition, Telemetry telemetry)
    {
        return condition.TelemetryTypeMatchingType switch
        {
            TelemetryTypeMatchingType.AllTypes => true,
            TelemetryTypeMatchingType.SpecifiedTypes => (condition.TelemetryTypeMatchingSpecifiedTypes ?? string.Empty)
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Any(x => telemetry.Type == x),
            _ => false
        };
    }
}