using Data;

namespace Services.Processing.Actions;

public interface IProcessAction
{
    Task ExecuteAsync(RuleAction action, ApplicationContext context, CancellationToken cancellationToken);
}