using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Processing;

public class ProcessSendInstructionAction(AppDbContext dbContext,ISystemClock clock) : IProcessAction
{
    public async Task ExecuteAsync(RuleAction action, TenantContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<ProcessSendInstructionAction>();

        if (action.SendInstructionType is null)
        {
            log.Error(
                "SendInstructionType is null for send instruction action {RuleActionId} in tenant {TenantId}",
                action.Id,
                context.Tenant.Id);
            return;
        }
        
        var groups = (action.SendInstructionDeviceGroups ?? string.Empty)
            .Split(",")
            .Select(x => x.Trim());
        var devices = action.SendInstructionTargetDeviceType switch
        {
            RuleActionSendInstructionTargetDeviceType.SingleDevice => await dbContext.Devices
                .Where(x => x.Id == action.SendInstructionDeviceId && x.Tenant == context.Tenant)
                .ToListAsync(cancellationToken),
            RuleActionSendInstructionTargetDeviceType.DeviceGroups => await dbContext.Devices
                .Where(x => x.TenantId == context.Tenant.Id && groups.Contains(x.GroupName))
                .ToListAsync(cancellationToken),
            _ => throw new NotImplementedException(
                $"Unimplemented send instruction target device type ${action.SendInstructionTargetDeviceType}")
        };

        if (devices.Count == 0)
        {
            log.Warning(
                "No devices match action {RuleActionId} in rule {RuleId} in tenant {TenantId}",
                action.Id,
                action.RuleId,
                context.Tenant.Id);
            return;
        }

        foreach (var device in devices)
        {
            var result = await dbContext.Instructions.AddAsync(new Instruction
            {
                Device = device,
                Type = action.SendInstructionType,
                Value = action.SendInstructionValue,
                Payload = action.SendInstructionPayload,
                CreatedAt = clock.UtcNow
            }, cancellationToken);

            log.Information(
                "Sent instruction {InstructionId} to device {DeviceId} for rule {RuleId} action {RuleActionId} in tenant {TenantId}",
                result.Entity.Id,
                device.Id,
                action.RuleId,
                action.Id,
                context.Tenant.Id);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}