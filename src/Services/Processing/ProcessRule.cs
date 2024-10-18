using Data;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Services.Processing.Actions;
using Services.Processing.Conditions;

namespace Services.Processing;

public interface IProcessRule
{
    Task ExecuteAsync(Rule rule, ApplicationContext context, CancellationToken cancellationToken);
}

public class ProcessRule(IServiceProvider serviceProvider) : IProcessRule
{
    public async Task ExecuteAsync(Rule rule, ApplicationContext context, CancellationToken cancellationToken)
    {
        if (rule.Conditions.Count == 0 || rule.Actions.Count == 0)
        {
            return;
        }

        var log = Log.ForContext<ProcessRule>();

        foreach (var condition in rule.Conditions)
        {
            var checkCondition = serviceProvider.GetKeyedService<ICheckCondition>(condition.Type);

            if (checkCondition is null)
            {
                log.Error(
                    "Unimplemented condition processor when executing condition {RuleConditionId} for type {RuleConditionType}",
                    condition.Id,
                    condition.Type);
                return;
            }
            
            var passed = await checkCondition.ExecuteAsync(condition, context, cancellationToken);
            
            if (!passed)
            {
                return;
            }
        }

        foreach (var action in rule.Actions)
        {
            var processAction = serviceProvider.GetKeyedService<IProcessAction>(action.Type);

            if (processAction is null)
            {
                log.Error(
                    "Unimplemented action processor when executing rule action {RuleActionId} for type {RuleActionType}",
                    action.Id,
                    action.Type);
                return;
            }

            await processAction.ExecuteAsync(action, context, cancellationToken);
        }
    }
}