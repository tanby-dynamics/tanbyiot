using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Services.Devices;
using Services.Instructions;
using Services.Options;
using Services.Processing;
using Services.Processing.Actions;
using Services.Processing.Conditions;
using Services.Telemetries;
using Services.Queueing;
using Services.Rules;
using Services.Rules.Actions;
using Services.Rules.Conditions;
using Services.Tenants;
using Services.Users;

namespace Services;

public static class ConfigureServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureStorageOptions>(options => configuration.GetSection("AzureStorage").Bind(options));
        services.AddKeyedScoped<IProcessAction, ProcessSendInstructionAction>(RuleActionType.SendInstruction);
        services.AddKeyedScoped<IProcessAction, ProcessSetStateAction>(RuleActionType.SetState);
        services.AddKeyedScoped<ICheckCondition, CheckTelemetryTypesCondition>(RuleConditionType.TelemetryTypes);
        services.AddKeyedScoped<ICheckCondition, CheckTenantStateCondition>(RuleConditionType.State);
        services.AddScoped<IQueueManager, QueueManager>();
        services.AddScoped<IGetAllDevicesForTenant, GetAllDevicesForTenant>();
        services.AddScoped<IAddDevice, AddDevice>();
        services.AddScoped<IAddTelemetry, AddTelemetry>();
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IConnectDevice, ConnectDevice>();
        services.AddScoped<IProcessTelemetry, ProcessTelemetry>();
        services.AddScoped<IGetTelemetryForDevice, GetTelemetryForDevice>();
        services.AddScoped<IGetInstructionsForDevice, GetInstructionsForDevice>();
        services.AddScoped<IGetDevice, GetDevice>();
        services.AddScoped<IPollForInstructions, PollForInstructions>();
        services.AddScoped<IGetRulesForTenant, GetRulesForTenant>();
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
        services.AddScoped<IUpdateRule, UpdateRule>();
        services.AddScoped<IAddUser, AddUser>();
        services.AddScoped<IGetUserByExternalId, GetUserByExternalId>();
        services.AddScoped<IAddTenantForUser, AddTenantForUser>();
        services.AddScoped<ISetCurrentTenantForUser, SetCurrentTenantForUser>();
        services.AddScoped<IGetCurrentTenantIdForUser, GetCurrentTenantIdForUser>();
        services.AddScoped<IValidateTenantForUser, ValidateTenantForUser>();
        services.AddScoped<IValidateDeviceInTenant, ValidateDeviceInTenant>();
        services.AddScoped<IValidateRuleInTenant, ValidateRuleInTenant>();
        services.AddScoped<IValidateConditionInRule, ValidateConditionInRule>();
        services.AddScoped<IValidateActionInRule, ValidateActionInRule>();
        services.AddScoped<IGetUsers, GetUsers>();
        services.AddScoped<ISetUserEmail, SetUserEmail>();
    }
}