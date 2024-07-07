using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.TenantStates;
using Services.Users;

namespace tanbyiot.Server.Features.TenantStates;

[ApiController]
[Route("/api/tenants/{tenantId:guid}/states")]
[Authorize]
public class TenantStatesController(
    IValidateTenantForUser validateTenantForUser,
    IGetStatesForTenant getStatesForTenant,
    IUpdateTenantState updateTenantState,
    IValidateTenantStateInTenant validateTenantStateInTenant,
    IDeleteTenantState deleteTenantState,
    IAddTenantState addTenantState) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<TenantStateDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStates(Guid tenantId, CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        var states = await getStatesForTenant.ExecuteAsync(tenantId, cancellationToken);

        return Ok(states);
    }

    [HttpPut("{tenantRuleId:guid}")]
    [ProducesResponseType<TenantStateDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTenantState(Guid tenantId, Guid tenantRuleId, UpdateTenantStateArgs args,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        if (!await ValidateStateInTenant(tenantId, tenantRuleId, cancellationToken))
        {
            return NotFound();
        }

        var state = await updateTenantState.ExecuteAsync(tenantRuleId, args, cancellationToken);

        return Ok(state);
    }

    [HttpDelete("{tenantStateId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteTenantState(Guid tenantId, Guid tenantStateId,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken) ||
            !await ValidateStateInTenant(tenantId, tenantStateId, cancellationToken))
        {
            return NotFound();
        }

        await deleteTenantState.ExecuteAsync(tenantStateId, cancellationToken);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType<TenantStateDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTenantState(Guid tenantId, AddTenantStateArgs args,
        CancellationToken cancellationToken)
    {
        if (!await ValidateTenant(tenantId, cancellationToken))
        {
            return NotFound();
        }

        var tenantState = await addTenantState.ExecuteAsync(tenantId, args, cancellationToken);

        return Ok(tenantState);
    }
    
    private async Task<bool> ValidateTenant(Guid tenantId, CancellationToken cancellationToken)
    {
        var isTenantValidForUser = await validateTenantForUser.ExecuteAsync(
            tenantId, 
            HttpContext.User.Identity?.Name!,
            cancellationToken);

        if (isTenantValidForUser) return true;
        
        var log = Log.ForContext<TenantStatesController>();
        log.Warning(
            "Tenant {TenantId} not valid for user {ExternalId}",
            tenantId,
            HttpContext.User.Identity?.Name!);
        
        return false;
    }

    private async Task<bool> ValidateStateInTenant(Guid tenantId, Guid tenantStateId,
        CancellationToken cancellationToken)
    {
        if (await validateTenantStateInTenant.ExecuteAsync(tenantId, tenantStateId, cancellationToken))
        {
            return true;
        }

        var log = Log.ForContext<TenantStatesController>();
        log.Warning(
            "Tenant state {TenantStateId} not in tenant {TenantId}",
            tenantId,
            tenantStateId);

        return false;
    }
}