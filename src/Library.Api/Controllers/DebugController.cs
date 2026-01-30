using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/debug")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _config;
    public DebugController(IConfiguration config) => _config = config;

    [HttpGet("conn")]
    public IActionResult Conn()
    {
        var cs = _config.GetConnectionString("DefaultConnection");
        return Ok(new
        {
            hasConnectionString = !string.IsNullOrWhiteSpace(cs),
            startsWithHost = cs?.Contains("Host=") == true
        });
    }
}
