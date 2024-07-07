import {RuleActionType, RuleConditionType} from "../api/enums.ts";

export function getLabelForRuleConditionType(type: RuleConditionType) {
    switch (type) {
        case RuleConditionType.TelemetryTypes:
            return "Telemetry type";
        case RuleConditionType.Value:
            return "Telemetry value";
        case RuleConditionType.Payload:
            return "Telemetry payload";
        case RuleConditionType.DeviceId:
            return "Telemetry received from device";
        case RuleConditionType.Group:
            return "Telemetry received from group";
        case RuleConditionType.State:
            return "Tenant state";
        default:
            return `Unknown condition type: ${type}`;
    }
}

export function getLabelForRuleActionType(type: RuleActionType) {
    switch (type) {
        case RuleActionType.SendInstruction:
            return "Send instruction";
        case RuleActionType.TriggerWebhook:
            return "Trigger webhook";
        case RuleActionType.SendEmail:
            return "Send email";
        case RuleActionType.SendSMS:
            return "Send SMS";
        case RuleActionType.SetState:
            return "Set tenant state";
        default:
            return `Unknown action type: ${type}`;
    }
}