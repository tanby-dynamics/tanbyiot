using Data;

namespace Services.Processing.Conditions;

public class CheckTelemetryCondition : ICheckCondition
{
    public Task<bool> ExecuteAsync(RuleCondition condition, ApplicationContext context,
        CancellationToken cancellationToken)
    {
        if (context.CurrentTelemetry is null)
        {
            return Task.FromResult(false);
        }

        if (!CheckConditionForDevice.Execute(condition, context.CurrentTelemetry))
        {
            return Task.FromResult(false);
        }

        if (!CheckConditionForTelemetryTypes.Execute(condition, context.CurrentTelemetry))
        {
            return Task.FromResult(false);
        }

        if (!CheckConditionForTelemetryValue.Execute(condition, context.CurrentTelemetry))
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}