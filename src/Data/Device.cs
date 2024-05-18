﻿namespace Data;

public class Device
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public Tenant Tenant { get; init; }
    public string Name { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public DateTimeOffset? LastConnected { get; set; }
}