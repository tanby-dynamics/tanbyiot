using Data;

namespace Services.Processing.Conditions;

public interface ICheckCondition
{
    Task<bool> ExecuteAsync(RuleCondition condition, ApplicationContext context, CancellationToken cancellationToken);
}