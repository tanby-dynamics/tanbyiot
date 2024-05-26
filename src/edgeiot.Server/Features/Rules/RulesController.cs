﻿using edgeiot.Server.Features.Devices;
using Microsoft.AspNetCore.Mvc;
using Services.Rules;

namespace edgeiot.Server.Features.Rules;

[ApiController]
[Route("/api/rules")]
public class RulesController(
    IGetAllRulesForTenant getAllRulesForTenant,
    IAddRule addRule,
    IGetRuleDetail getRuleDetail) : ControllerBase
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
}