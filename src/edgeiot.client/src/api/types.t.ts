import { Moment } from "moment";

export type Device = {
    id: string;
    name: string;
    groupName: string;
    lastConnected: string | Moment | null;
}

export type Telemetry = {
    id: string;
    deviceId: string;
    type: string;
    value: string | null;
    payload: string | null;
    receivedAt: string | Moment;
}

export type Instruction = {
    id: string;
    deviceId: string;
    type: string;
    value: string | null;
    payload: string | null;
    createdAt: string | Moment;
    sentAt: string | Moment | null;
}

export type Rule = {
    id: string;
    name: string;
    enabled: boolean;
    createdAt: string | Moment;
}

export type RuleDetail = Rule & {
    conditions: RuleCondition[];
    actions: RuleAction[];
}

export type RuleCondition = {
    id: string;
    createdAt: string | Moment;
    type: RuleConditionType;
}

export enum RuleConditionType {
    telemetryType = "TelemetryType",
    value = "Value",
    payload = "Payload",
    deviceId = "DeviceId",
    group = "Group"
}

export type RuleAction = {
    id: string;
    createdAt: string | Moment;
    type: RuleActionType;
}

export enum RuleActionType {
    sendInstruction = "SendInstruction",
    triggerWebhook = "TriggerWebhook",
    sendEmail = "SendEmail",
    sendSMS = "SendSMS"
}
