using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Services.ApplicationStates;
using Services.Devices;
using Services.Instructions;
using Services.Messaging;
using Services.Processing;
using Services.Processing.Actions;
using Services.Processing.Conditions;
using Services.Telemetries;
using Services.Rules;
using Services.Rules.Actions;
using Services.Rules.Conditions;

namespace Services;

public static class ConfigureServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeyedScoped<IProcessAction, ProcessSendInstructionAction>(RuleActionType.SendInstruction);
        services.AddKeyedScoped<IProcessAction, ProcessSetStateAction>(RuleActionType.SetState);
        services.AddKeyedScoped<ICheckCondition, CheckTelemetryCondition>(RuleConditionType.Telemetry);
        services.AddKeyedScoped<ICheckCondition, CheckApplicationStateCondition>(RuleConditionType.State);
        services.AddScoped<IMessageManager, MessageManager>();
        services.AddScoped<IGetAllDevices, GetAllDevices>();
        services.AddScoped<IAddDevice, AddDevice>();
        services.AddScoped<IAddTelemetry, AddTelemetry>();
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IConnectDevice, ConnectDevice>();
        services.AddScoped<IProcessTelemetry, ProcessTelemetry>();
        services.AddScoped<IGetTelemetryForDevice, GetTelemetryForDevice>();
        services.AddScoped<IGetInstructionsForDevice, GetInstructionsForDevice>();
        services.AddScoped<IGetDevice, GetDevice>();
        services.AddScoped<IPollForInstructions, PollForInstructions>();
        services.AddScoped<IGetRules, GetRules>();
        services.AddScoped<IAddRule, AddRule>();
        services.AddScoped<IGetRuleDetail, GetRuleDetail>();
        services.AddScoped<IAddRuleCondition, AddRuleCondition>();
        services.AddScoped<IAddRuleAction, AddRuleAction>();
        services.AddScoped<IDeleteRuleCondition, DeleteRuleCondition>();
        services.AddScoped<IDeleteRuleAction, DeleteRuleAction>();
        services.AddScoped<IUpdateRuleCondition, UpdateRuleCondition>();
        services.AddScoped<IUpdateRuleAction, UpdateRuleAction>();
        services.AddScoped<IApplicationContextFactory, ApplicationContextFactory>();
        services.AddScoped<IProcessRule, ProcessRule>();
        services.AddScoped<IUpdateRule, UpdateRule>();
        services.AddScoped<IValidateConditionInRule, ValidateConditionInRule>();
        services.AddScoped<IValidateActionInRule, ValidateActionInRule>();
        services.AddScoped<IGetApplicationStates, GetApplicationApplicationStates>();
        services.AddScoped<IUpdateApplicationState, UpdateApplicationState>();
        services.AddScoped<IDeleteApplicationState, DeleteApplicationState>();
        services.AddScoped<IAddApplicationState, AddApplicationState>();
    }
}