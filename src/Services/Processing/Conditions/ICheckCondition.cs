using Data;

namespace Services.Processing.Conditions;

public interface ICheckCondition
{
    Task<bool> ExecuteAsync(RuleCondition condition, TenantContext context, CancellationToken cancellationToken);
}