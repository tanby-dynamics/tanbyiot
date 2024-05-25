using edgeiot.Server.Features.Devices;
using Microsoft.AspNetCore.Mvc;
using Services.Rules;

namespace edgeiot.Server.Features.Rules;

[ApiController]
[Route("/api/rules")]
public class RulesController(
    IGetAllRulesForTenant getAllRulesForTenant) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<RuleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRules(CancellationToken cancellationToken)
    {
        // TODO take tenant ID from request, check for authorization
        var rules = await getAllRulesForTenant.ExecuteAsync(DevicesController.TenantId, cancellationToken);
        
        return Ok(rules);
    }
}