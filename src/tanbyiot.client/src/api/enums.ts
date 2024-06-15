export enum RuleConditionType {
    TelemetryTypes = "TelemetryTypes",
    Value = "Value",
    Payload = "Payload",
    DeviceId = "DeviceId",
    Group = "Group"
}

export enum RuleActionType {
    SendInstruction = "SendInstruction",
    TriggerWebhook = "TriggerWebhook",
    SendEmail = "SendEmail",
    SendSMS = "SendSMS"
}

export enum RuleActionSendInstructionTargetDeviceType {
    DeviceGroups = "DeviceGroups",
    SingleDevice = "SingleDevice"
}

export enum RuleConditionComparisonOperationType {
    Equals = "Equals",
    NotEquals = "NotEquals",
    LessThan = "LessThan"
}

export enum RuleConditionConversionType {
    Number = "Number",
    String = "String",
    Boolean = "Boolean"
}

export enum SubscriptionLevel {
    Developer = "Developer",
    Startup = "Startup",
    Business = "Business",
    Industry = "Industry"
}

