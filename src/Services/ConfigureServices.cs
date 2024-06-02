using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Services.Devices;
using Services.Instructions;
using Services.Options;
using Services.Processing;
using Services.Telemetries;
using Services.Queueing;
using Services.Rules;

namespace Services;

public static class ConfigureServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureStorageOptions>(options => configuration.GetSection("AzureStorage").Bind(options));
        services.AddScoped<IQueueManager, QueueManager>();
        services.AddScoped<IGetAllDevicesForTenant, GetAllDevicesForTenant>();
        services.AddScoped<IAddDevice, AddDevice>();
        services.AddScoped<IAddTelemetry, AddTelemetry>();
        services.AddScoped<IValidateDevice, ValidateDevice>();
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IConnectDevice, ConnectDevice>();
        services.AddScoped<IProcessTelemetry, ProcessTelemetry>();
        services.AddScoped<IGetTelemetryForDevice, GetTelemetryForDevice>();
        services.AddScoped<IGetInstructionsForDevice, GetInstructionsForDevice>();
        services.AddScoped<IGetDevice, GetDevice>();
        services.AddScoped<IPollForInstructions, PollForInstructions>();
        services.AddScoped<IGetAllRulesForTenant, GetAllRulesForTenant>();
        services.AddScoped<IAddRule, AddRule>();
        services.AddScoped<IGetRuleDetail, GetRuleDetail>();
        services.AddScoped<IAddRuleCondition, AddRuleCondition>();
        services.AddScoped<IAddRuleAction, AddRuleAction>();
        services.AddScoped<IDeleteRuleCondition, DeleteRuleCondition>();
        services.AddScoped<IDeleteRuleAction, DeleteRuleAction>();
        services.AddScoped<IUpdateRuleCondition, UpdateRuleCondition>();
        services.AddScoped<IUpdateRuleAction, UpdateRuleAction>();
        services.AddScoped<ITenantContextFactory, TenantContextFactory>();
        services.AddScoped<IProcessRule, ProcessRule>();
        services.AddKeyedScoped<IProcessAction, ProcessSendInstructionAction>(RuleActionType.SendInstruction);
        services.AddKeyedScoped<ICheckCondition, CheckTelemetryTypesCondition>(RuleConditionType.TelemetryTypes);
    }
}