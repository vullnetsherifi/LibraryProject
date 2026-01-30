using Library.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("db")]
    public async Task<IActionResult> Db([FromServices] AppDbContext db)
    {
        var canConnect = await db.Database.CanConnectAsync();
        var pending = await db.Database.GetPendingMigrationsAsync();
        return Ok(new
        {
            canConnect,
            pendingMigrations = pending.ToArray()
        });
    }
}
