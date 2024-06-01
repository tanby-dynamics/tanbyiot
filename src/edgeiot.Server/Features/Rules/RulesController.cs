using edgeiot.Server.Features.Devices;
using Microsoft.AspNetCore.Mvc;
using Services.Rules;

namespace edgeiot.Server.Features.Rules;

[ApiController]
[Route("/api/rules")]
public class RulesController(
    IGetAllRulesForTenant getAllRulesForTenant,
    IAddRule addRule,
    IGetRuleDetail getRuleDetail,
    IAddRuleCondition addRuleCondition,
    IAddRuleAction addRuleAction) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<RuleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRules(CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var rules = await getAllRulesForTenant.ExecuteAsync(DevicesController.TenantId, cancellationToken);
        
        return Ok(rules);
    }

    [HttpPost]
    [ProducesResponseType<RuleDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddRule(AddRuleRequestDto request, CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var rule = await addRule.ExecuteAsync(DevicesController.TenantId, request.Name, cancellationToken);

        return Ok(rule);
    }
    
    [HttpGet("{ruleId:guid}")]
    [ProducesResponseType<RuleDetailDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRuleDetail(Guid ruleId, CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var rule = await getRuleDetail.ExecuteAsync(DevicesController.TenantId, ruleId, cancellationToken);
        
        return Ok(rule);
    }

    [HttpPost("{ruleId:guid}/conditions")]
    [ProducesResponseType<RuleConditionDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCondition(Guid ruleId, AddRuleConditionRequestDto request,
        CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var condition = await addRuleCondition.ExecuteAsync(ruleId, request.Type, cancellationToken);

        return Ok(condition);
    }

    [HttpPost("{ruleId:guid}/actions")]
    [ProducesResponseType<RuleActionDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAction(Guid ruleId, AddRuleActionRequestDto request,
        CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var action = await addRuleAction.ExecuteAsync(ruleId, request.Type, cancellationToken);

        return Ok(action);
    }
}