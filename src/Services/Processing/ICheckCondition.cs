using Data;

namespace Services.Processing;

public interface ICheckCondition
{
    Task<bool> ExecuteAsync(RuleCondition condition, TenantContext context, CancellationToken cancellationToken);
}