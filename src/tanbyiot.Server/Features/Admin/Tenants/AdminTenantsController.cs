using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tanbyiot.Server.Features.Admin.Tenants;

[ApiController]
[Route("/api/admin/tenants")]
[Authorize("admin:all")]
public class AdminTenantsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
    public IActionResult GetAllThings()
    {
        return Ok(new[] { "a", "b", "c" });
    }
}