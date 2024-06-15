import { Moment } from "moment";
import {
    RuleActionSendInstructionTargetDeviceType,
    RuleActionType,
    RuleConditionComparisonOperationType,
    RuleConditionConversionType,
    RuleConditionType, SubscriptionLevel
} from "./enums.ts";

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
    updatedAt: string | Moment | null;
}

export type RuleDetail = Rule & {
    conditions: RuleCondition[];
    actions: RuleAction[];
}

export type UpdateRuleArgs = {
    name: string;
    enabled: boolean;
}

export type RuleCondition = {
    id: string;
    createdAt: string | Moment;
    type: RuleConditionType;
    comparisonValue: string | null;
    comparisonOperation: RuleConditionComparisonOperationType | null;
    payloadPath: string | null;
    conversion: RuleConditionConversionType | null;
    updatedAt: string | Moment | null;
}

export type RuleAction = {
    id: string;
    createdAt: string | Moment;
    type: RuleActionType;
    updatedAt: string | Moment;
    sendInstructionType: string | null;
    sendInstructionValue: string | null;
    sendInstructionPayload: string | null;
    sendInstructionDeviceId: string | null;
    sendInstructionDeviceGroups: string | null;
    sendInstructionTargetDeviceType: RuleActionSendInstructionTargetDeviceType | null;
}

export type UpdateRuleConditionArgs = {
    comparisonValue: string;
    comparisonOperation: RuleConditionComparisonOperationType | null;
    payloadPath: string | null;
    conversionType: RuleConditionConversionType | null;
}

export type UpdateRuleActionArgs = {
    sendInstructionType: string | null;
    sendInstructionValue: string | null;
    sendInstructionPayload: string | null;
    sendInstructionDeviceId: string | null;
    sendInstructionDeviceGroups: string | null;
    sendInstructionTargetDeviceType: string | null;
}

export type Tenant = {
    id: string;
    name: string;
}

export type SystemUser = {
    id: string;
    externalId: string;
    tenants: Tenant[];
    currentTenant: Tenant;
}

export type AddTenantArgs = {
    name: string;
    subscriptionLevel: SubscriptionLevel;
}