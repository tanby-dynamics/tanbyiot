import {RuleConditionType} from "../api/types.t.ts";

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
        case RuleConditionType.deviceId: return "Device ID";
        case RuleConditionType.group: return "Device group";
        case RuleConditionType.payload: return "Telemetry payload";
        case RuleConditionType.telemetryTypes: return "Telemetry type";
        case RuleConditionType.value: return "Telemetry value";
        default: return `Unknown type {type}`;
    }
}