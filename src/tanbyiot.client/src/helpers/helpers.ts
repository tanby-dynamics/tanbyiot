import {RuleActionType, RuleConditionType} from "../api/enums.ts";

export function jsonTryParse(json: string | null): any | null {
    if (!json) {
        return null;
    }
    try {
        return JSON.parse(json);
    } catch {
        return null;
    }
}

export function formatRuleConditionType(type: RuleConditionType) {
    switch (type) {
        case RuleConditionType.DeviceId: return "Device ID";
        case RuleConditionType.Group: return "Device group";
        case RuleConditionType.Payload: return "Telemetry payload";
        case RuleConditionType.TelemetryTypes: return "Telemetry type";
        case RuleConditionType.Value: return "Telemetry value";
        default: return `Unknown type {type}`;
    }
}

export function formatRuleActionType(type: RuleActionType) {
    switch (type) {
        case RuleActionType.SendInstruction: return "Send instruction";
        case RuleActionType.TriggerWebhook: return "Trigger webhook";
        case RuleActionType.SendEmail: return "Send email";
        case RuleActionType.SendSMS: return "Send SMS";
        case RuleActionType.SetState: return "Set tenant state";
        default: return `Unknown type {type}`;
    }
}