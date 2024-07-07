using Data;

namespace Services.Processing.Actions;

public interface IProcessAction
{
    Task ExecuteAsync(RuleAction action, TenantContext context, CancellationToken cancellationToken);
}