using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class RuleAction
{
    public Guid Id { get; init; }
    public Guid RuleId { get; init; }
    public RuleActionType Type { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    [MaxLength(128)] public string? SendInstructionType { get; set; }
    [MaxLength(128)] public string? SendInstructionValue { get; set; }
    [MaxLength(128)] public string? Key { get; set; }
    [MaxLength(4000)] public string? Payload { get; set; }
    public Guid? SendInstructionDeviceId { get; set; }
    [MaxLength(256)] public string? SendInstructionDeviceGroups { get; set; }
    public RuleActionSendInstructionTargetDeviceType? SendInstructionTargetDeviceType { get; set; }
    public virtual Rule Rule { get; init; } = default!;
    [MaxLength(256)] public string? SendEmailSenderEmail { get; set; }
    [MaxLength(256)] public string? SendEmailSenderName { get; set; }
    [MaxLength(256)] public string? SendEmailToEmail { get; set; }
    [MaxLength(256)] public string? SendEmailSubject { get; set; }
    [MaxLength(4000)] public string? SendEmailBody { get; set; }
}