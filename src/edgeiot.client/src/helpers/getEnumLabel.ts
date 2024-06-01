import {RuleActionType, RuleConditionType} from "../api/types.t.ts";

export function getLabelForRuleConditionType(type: RuleConditionType) {
    switch (type) {
        case RuleConditionType.telemetryTypes:
            return "Telemetry type";
        case RuleConditionType.value:
            return "Value";
        case RuleConditionType.payload:
            return "Payload";
        case RuleConditionType.deviceId:
            return "Device ID";
        case RuleConditionType.group:
            return "Group";
        default:
            return `Unknown condition type: ${type}`;
    }
}

export function getLabelForRuleActionType(type: RuleActionType) {
    switch (type) {
        case RuleActionType.sendInstruction:
            return "Send instruction";
        case RuleActionType.triggerWebhook:
            return "Trigger webhook";
        case RuleActionType.sendEmail:
            return "Send email";
        case RuleActionType.sendSMS:
            return "Send SMS";
        default:
            return `Unknown action type: ${type}`;
    }
}