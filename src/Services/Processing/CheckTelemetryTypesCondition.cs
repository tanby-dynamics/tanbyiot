using Data;

namespace Services.Processing;

public class CheckTelemetryTypesCondition : ICheckCondition
{
    public Task<bool> ExecuteAsync(RuleCondition condition, TenantContext context, CancellationToken cancellationToken)
    {
        if (context.CurrentTelemetry is null)
        {
            return Task.FromResult(false);
        }
        
        var types = (condition.ComparisonValue ?? string.Empty)
            .Split(",")
            .Select(x => x.Trim())
            .ToList();

        return Task.FromResult(types.Any(x => context.CurrentTelemetry.Type == x));
    }
}