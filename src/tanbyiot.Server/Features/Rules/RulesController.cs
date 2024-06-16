using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Rules;
using Services.Users;

namespace tanbyiot.Server.Features.Rules;

[ApiController]
[Route("/api/tenants/{tenantId:guid}/rules")]
[Authorize]
public class RulesController(
    IGetRulesForTenant getRulesForTenant,
    IAddRule addRule,
    IGetRuleDetail getRuleDetail,
    IAddRuleCondition addRuleCondition,
    IAddRuleAction addRuleAction,
    IDeleteRuleCondition deleteRuleCondition,
    IDeleteRuleAction deleteRuleAction,
    IUpdateRuleCondition updateRuleCondition,
    IUpdateRuleAction updateRuleAction,
    IUpdateRule updateRule,
    IValidateTenantForUser validateTenantForUser,
    IValidateRuleInTenant validateRuleInTenant,
    IValidateActionInRule validateActionInRule,
    IValidateConditionInRule validateConditionInRule) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<RuleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRules(Guid tenantId, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        var rules = await getRulesForTenant.ExecuteAsync(tenantId, cancellationToken);
        
        return Ok(rules);
    }

    [HttpPost]
    [ProducesResponseType<RuleDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddRule(Guid tenantId, AddRuleRequestDto request, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        var rule = await addRule.ExecuteAsync(tenantId, request.Name, cancellationToken);

        return Ok(rule);
    }

    [HttpPut("{ruleId:guid}")]
    [ProducesResponseType<RuleDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRule(Guid tenantId, Guid ruleId, UpdateRuleArgs args, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        var rule = await updateRule.ExecuteAsync(ruleId, args, cancellationToken);

        return Ok(rule);
    }
    
    [HttpGet("{ruleId:guid}")]
    [ProducesResponseType<RuleDetailDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRuleDetail(Guid tenantId, Guid ruleId, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        var ruleDetail = await getRuleDetail.ExecuteAsync(ruleId, cancellationToken);
        
        return Ok(ruleDetail);
    }
    
    async Task<bool> ValidateRuleInTenant(Guid tenantId, Guid ruleId, CancellationToken cancellationToken)
    {
        if (!await validateRuleInTenant.ExecuteAsync(tenantId, ruleId, cancellationToken))
        {
            var log = Log.ForContext<RulesController>();
            log.Warning("Rule {RuleId} not in tenant {TenantId}", ruleId, tenantId);
            return false;
        }

        return true;
    }

    [HttpPost("{ruleId:guid}/conditions")]
    [ProducesResponseType<RuleConditionDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCondition(Guid tenantId, Guid ruleId, AddRuleConditionRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }
        
        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        var condition = await addRuleCondition.ExecuteAsync(ruleId, request.Type, cancellationToken);

        return Ok(condition);
    }

    [HttpPut("{ruleId:guid}/conditions")]
    [ProducesResponseType<RuleConditionDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCondition(
        Guid tenantId, 
        Guid ruleId, 
        UpdateRuleConditionArgs args, 
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateConditionInRule(ruleId, args.RuleConditionId, cancellationToken))
        {
            return NotFound();
        }
        
        var condition = await updateRuleCondition.ExecuteAsync(args, cancellationToken);

        return Ok(condition);
    }

    [HttpDelete("{ruleId:guid}/conditions/{ruleConditionId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCondition(Guid tenantId, Guid ruleId, Guid ruleConditionId,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateConditionInRule(ruleId, ruleConditionId, cancellationToken))
        {
            return NotFound();
        }
        
        await deleteRuleCondition.ExecuteAsync(ruleConditionId, cancellationToken);

        return Ok();
    }

    [HttpPost("{ruleId:guid}/actions")]
    [ProducesResponseType<RuleActionDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAction(Guid tenantId, Guid ruleId, AddRuleActionRequestDto request,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        var action = await addRuleAction.ExecuteAsync(ruleId, request.Type, cancellationToken);

        return Ok(action);
    }

    [HttpPut("{ruleId:guid}/actions/{ruleActionId:guid}")]
    [ProducesResponseType<RuleActionDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAction(Guid tenantId, Guid ruleId, Guid ruleActionId,
        UpdateRuleActionArgs request, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateActionInRule(ruleId, ruleActionId, cancellationToken))
        {
            return NotFound();
        }

        var action = await updateRuleAction.ExecuteAsync(ruleActionId, request, cancellationToken);

        return Ok(action);
    }

    [HttpDelete("{ruleId:guid}/actions/{ruleActionId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAction(Guid tenantId, Guid ruleId, Guid ruleActionId, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateRuleInTenant(tenantId, ruleId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateActionInRule(ruleId, ruleActionId, cancellationToken))
        {
            return NotFound();
        }

        await deleteRuleAction.ExecuteAsync(ruleActionId, cancellationToken);

        return Ok();
    }

    private async Task<bool> ValidateTenant(Guid tenantId, CancellationToken cancellationToken)
    {
        var isTenantValidForUser = await validateTenantForUser.ExecuteAsync(
            tenantId, 
            HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (!isTenantValidForUser)
        {
            var log = Log.ForContext<RulesController>();
            log.Warning("Tenant {TenantId} not valid for user {ExternalId}",
                tenantId,
                HttpContext.User.Identity?.Name!,
                cancellationToken);
            return false;
        }

        return true;
    }
    
    private async Task<bool> ValidateConditionInRule(Guid ruleId, Guid ruleConditionId, CancellationToken cancellationToken)
    {
        if (!await validateConditionInRule.ExecuteAsync(ruleId, ruleConditionId, cancellationToken))
        {
            var log = Log.ForContext<RulesController>();
            log.Warning(
                "Rule condition {RuleConditionId} not in rule {RuleId}",
                ruleConditionId,
                ruleId);
            return false;
        }

        return true;
    }

    private async Task<bool> ValidateActionInRule(Guid ruleId, Guid ruleActionId, CancellationToken cancellationToken)
    {
        if (!await validateActionInRule.ExecuteAsync(ruleId, ruleActionId, cancellationToken))
        {
            var log = Log.ForContext<RulesController>();
            log.Warning(
                "Rule action {RuleActionId} not in rule {RuleId}",
                ruleActionId,
                ruleId);
            return false;
        }

        return true;
    }
}