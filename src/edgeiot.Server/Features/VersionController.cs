using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace edgeiot.Server.Features;

[ApiController]
[Route("/api/version")]
public class VersionController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult GetVersion()
    {
        var version = FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).FileVersion;
        
        return Ok(version);
    }
}