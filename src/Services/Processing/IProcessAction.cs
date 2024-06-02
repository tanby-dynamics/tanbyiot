using Data;

namespace Services.Processing;

public interface IProcessAction
{
    Task ExecuteAsync(RuleAction action, TenantContext context, CancellationToken cancellationToken);
}