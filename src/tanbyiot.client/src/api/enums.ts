export enum RuleConditionType {
    Telemetry = "Telemetry",
    State = "State"
}

export enum RuleActionType {
    SendInstruction = "SendInstruction",
    TriggerWebhook = "TriggerWebhook",
    SendEmail = "SendEmail",
    SendSMS = "SendSMS",
    SetState = "SetState"
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

export enum RuleConditionTelemetryTypeType {
    AllTypes = "AllTypes",
    SpecifiedTypes = "SpecifiedTypes"
}
