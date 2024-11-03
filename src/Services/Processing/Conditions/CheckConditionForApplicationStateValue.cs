using Data;
using Newtonsoft.Json.Linq;

namespace Services.Processing.Conditions;

public static class CheckConditionForApplicationStateValue
{
    public static bool Execute(RuleCondition condition, ApplicationState state)
    {
        // Get the value
        string? value = null;
        if (condition.ApplicationStateMatchingType == ApplicationStateMatchingType.UseValue)
        {
            value = state.Value;
        }

        if (condition is { ApplicationStateMatchingType: ApplicationStateMatchingType.ParsePayload, ApplicationStateMatchingPayloadPath: not null })
        {
            var payload = JObject.Parse(state.Value);
            var token = payload.SelectToken(condition.ApplicationStateMatchingPayloadPath);
            if (token != null)
            {
                value = token.ToString();
            }
        }

        if (value is null)
        {
            return false;
        }

        // Do a boolean comparison
        if (condition.ApplicationStateComparisonOperationType == ComparisonOperationType.IsTrue)
        {
            return value.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        if (condition.ApplicationStateComparisonOperationType == ComparisonOperationType.IsFalse)
        {
            return value.Equals("false", StringComparison.CurrentCultureIgnoreCase);
        }

        // Try to get value and matching value as decimal
        if (!decimal.TryParse(value, out var valueDecimal) ||
            !decimal.TryParse(condition.ApplicationStateMatchingValue, out var matchingValueDecimal))
        {
            // One or both are not decimals, so return a string comparison if it is an equals or not equals
            return condition.ApplicationStateComparisonOperationType switch
            {
                ComparisonOperationType.Equals => value == condition.ApplicationStateMatchingValue,
                ComparisonOperationType.NotEquals => value != condition.ApplicationStateMatchingValue,
                _ => false
            };
        }

        // Treat it as a numeric comparison
        return condition.ApplicationStateComparisonOperationType switch
        {
            ComparisonOperationType.Equals => valueDecimal == matchingValueDecimal,
            ComparisonOperationType.NotEquals => valueDecimal != matchingValueDecimal,
            ComparisonOperationType.GreaterThan => valueDecimal > matchingValueDecimal,
            ComparisonOperationType.LessThan => valueDecimal < matchingValueDecimal,
            _ => false
        };
    }
}