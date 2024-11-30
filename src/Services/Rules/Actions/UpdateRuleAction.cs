using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules.Actions;

public interface IUpdateRuleAction
{
    Task<RuleActionDto> ExecuteAsync(Guid ruleActionId, UpdateRuleActionArgs args,
        CancellationToken cancellationToken);
}

public class UpdateRuleAction(AppDbContext dbContext) : IUpdateRuleAction
{
    public async Task<RuleActionDto> ExecuteAsync(Guid ruleActionId, UpdateRuleActionArgs args,
        CancellationToken cancellationToken)
    {
        var action = await dbContext.RuleActions
            .SingleAsync(x => x.Id == ruleActionId, cancellationToken);

        action.SendInstructionType = args.SendInstructionType;
        action.SendInstructionValue = args.SendInstructionValue;
        action.Payload = args.Payload;
        action.SendInstructionDeviceId = args.SendInstructionDeviceId;
        action.SendInstructionDeviceGroups = args.SendInstructionDeviceGroups;
        action.SendInstructionTargetDeviceType = args.SendInstructionTargetDeviceType;
        action.Key = args.Key;
        action.SendEmailSenderEmail = args.SendEmailSenderEmail;
        action.SendEmailSenderName = args.SendEmailSenderName;
        action.SendEmailToEmail = args.SendEmailToEmail;
        action.SendEmailBody = args.SendEmailBody;
        action.SendEmailSubject = args.SendEmailSubject;

        await dbContext.SaveChangesAsync(cancellationToken);

        return RuleActionDto.FromEntity(action);
    }
}