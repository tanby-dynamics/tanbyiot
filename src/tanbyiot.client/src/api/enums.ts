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

export enum TelemetryTypeMatchingType {
    AllTypes = "AllTypes",
    SpecifiedTypes = "SpecifiedTypes"
}

export enum ApplicationStateMatchingType {
    UseValue = "UseValue",
    ParsePayload = "ParsePayload"
}

export enum ComparisonOperationType {
    Equals = "Equals",
    NotEquals = "NotEquals",
    LessThan = "LessThan",
    GreaterThan = "GreaterThan",
    IsTrue = "IsTrue",
    IsFalse = "IsFalse"
}

export enum DeviceMatchingType {
    AllDevices = "AllDevices",
    SingleDevice = "SingleDevice",
    DeviceGroups = "DeviceGroups"
}

export enum TelemetryValueMatchingType {
    AlwaysMatch = "AlwaysMatch",
    UseValue = "UseValue",
    ParsePayload = "ParsePayload"
}
