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
        case RuleConditionType.Telemetry: return "Telemetry";
        case RuleConditionType.State: return "Tenant state"
        default: return `Unknown type {type}`;
    }
}

export function formatRuleActionType(type: RuleActionType) {
    switch (type) {
        case RuleActionType.SendInstruction: return "Send instruction";
        case RuleActionType.TriggerWebhook: return "Trigger webhook";
        case RuleActionType.SendEmail: return "Send email";
        case RuleActionType.SendSMS: return "Send SMS";
        case RuleActionType.SetState: return "Set application state";
        default: return `Unknown type {type}`;
    }
}