using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Rules;

public interface IUpdateRuleAction
{
    Task<RuleActionDto> ExecuteAsync(Guid ruleId, Guid ruleActionId, UpdateRuleActionArgs args,
        CancellationToken cancellationToken);
}

public class UpdateRuleAction(AppDbContext dbContext) : IUpdateRuleAction
{
    public async Task<RuleActionDto> ExecuteAsync(Guid ruleId, Guid ruleActionId, UpdateRuleActionArgs args, CancellationToken cancellationToken)
    {
        var action = await dbContext.RuleActions
            .SingleAsync(x => x.RuleId == ruleId && x.Id == ruleActionId, cancellationToken);

        action.SendInstructionType = args.SendInstructionType;
        action.SendInstructionValue = args.SendInstructionValue;
        action.SendInstructionPayload = args.SendInstructionPayload;
        action.SendInstructionDeviceId = args.SendInstructionDeviceId;
        action.SendInstructionDeviceGroups = args.SendInstructionDeviceGroups;
        action.SendInstructionTargetDeviceType = args.SendInstructionTargetDeviceType;

        await dbContext.SaveChangesAsync(cancellationToken);

        return RuleActionDto.FromEntity(action);
    }
}