using Library.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AdminController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("migrate")]
    public async Task<IActionResult> Migrate()
    {
        var expected = _config["MIGRATE_KEY"];
        var provided = Request.Headers["X-MIGRATE-KEY"].ToString();

        if (string.IsNullOrWhiteSpace(expected) || provided != expected)
            return Unauthorized(new { message = "Unauthorized" });

        var pending = (await _db.Database.GetPendingMigrationsAsync()).ToArray();
        await _db.Database.MigrateAsync();

        return Ok(new { applied = pending });
    }
}
