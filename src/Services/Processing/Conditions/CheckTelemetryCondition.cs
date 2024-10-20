using Data;

namespace Services.Processing.Conditions;

public class CheckTelemetryCondition : ICheckCondition
{
    public Task<bool> ExecuteAsync(RuleCondition condition, ApplicationContext context, CancellationToken cancellationToken)
    {
        if (context.CurrentTelemetry is null)
        {
            return Task.FromResult(false);
        }

        if (!CheckForTelemetryTypes(condition, context))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    private static bool CheckForTelemetryTypes(RuleCondition condition, ApplicationContext context)
    {
        if (condition.TelemetryTypeMatchingType == TelemetryTypeMatchingType.AllTypes)
        {
            return true;
        }
        
        ArgumentNullException.ThrowIfNull(context.CurrentTelemetry);
        
        var types = (condition.ComparisonValue ?? string.Empty)
            .Split(",")
            .Select(x => x.Trim())
            .ToList();

        return types.Any(x => context.CurrentTelemetry.Type == x);
    }
}