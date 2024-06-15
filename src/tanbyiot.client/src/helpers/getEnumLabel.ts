import {RuleActionType, RuleConditionType} from "../api/enums.ts";

export function getLabelForRuleConditionType(type: RuleConditionType) {
    switch (type) {
        case RuleConditionType.TelemetryTypes:
            return "Telemetry type";
        case RuleConditionType.Value:
            return "Value";
        case RuleConditionType.Payload:
            return "Payload";
        case RuleConditionType.DeviceId:
            return "Device ID";
        case RuleConditionType.Group:
            return "Group";
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
        default:
            return `Unknown action type: ${type}`;
    }
}