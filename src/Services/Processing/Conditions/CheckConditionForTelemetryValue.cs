using Data;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Services.Processing.Conditions;

public static class CheckConditionForTelemetryValue
{
    public static bool Execute(RuleCondition condition, Telemetry telemetry)
    {
        if (condition.TelemetryValueMatchingType == TelemetryValueMatchingType.AlwaysMatch)
        {
            return true;
        }

        // Get the value
        string? value = null;
        if (condition.TelemetryValueMatchingType == TelemetryValueMatchingType.UseValue)
        {
            value = telemetry.Value;
        }

        if (condition is { TelemetryValueMatchingType: TelemetryValueMatchingType.ParsePayload, TelemetryValueMatchingPayloadPath: not null }
            && !string.IsNullOrEmpty(telemetry.Payload))
        {
            try
            {
                var payload = JObject.Parse(telemetry.Payload);
                var token = payload.SelectToken(condition.TelemetryValueMatchingPayloadPath);
                if (token != null)
                {
                    value = token.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error parsing JSON payload in telemetry {TelemetryId}", telemetry.Id);
                return false;
            }
        }

        if (value is null)
        {
            return false;
        }

        // Do a boolean comparison
        if (condition.TelemetryValueMatchingComparisonOperationType == ComparisonOperationType.IsTrue)
        {
            return value.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        if (condition.TelemetryValueMatchingComparisonOperationType == ComparisonOperationType.IsFalse)
        {
            return value.Equals("false", StringComparison.CurrentCultureIgnoreCase);
        }

        // Try to get value and matching value as decimal
        if (!decimal.TryParse(value, out var valueDecimal) ||
            !decimal.TryParse(condition.TelemetryValueMatchingValue, out var matchingValueDecimal))
        {
            // One or both are not decimals, so return a string comparison if it is an equals or not equals
            return condition.TelemetryValueMatchingComparisonOperationType switch
            {
                ComparisonOperationType.Equals => value == condition.TelemetryValueMatchingValue,
                ComparisonOperationType.NotEquals => value != condition.TelemetryValueMatchingValue,
                _ => false
            };
        }

        // Treat it as a numeric comparison
        return condition.TelemetryValueMatchingComparisonOperationType switch
        {
            ComparisonOperationType.Equals => valueDecimal == matchingValueDecimal,
            ComparisonOperationType.NotEquals => valueDecimal != matchingValueDecimal,
            ComparisonOperationType.GreaterThan => valueDecimal > matchingValueDecimal,
            ComparisonOperationType.LessThan => valueDecimal < matchingValueDecimal,
            _ => false
        };
    }
}