using Library.Api.Dtos;
using Library.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _service;
    public CategoriesController(CategoryService service) => _service = service;

    // Everyone can read
    [HttpGet]
    public async Task<ActionResult<List<CategoryReadDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{slug}")]
    public async Task<ActionResult<CategoryReadDto>> GetBySlug(string slug)
    {
        var item = await _service.GetBySlugAsync(slug);
        return item == null ? NotFound() : Ok(item);
    }

    // Admin only writes
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryReadDto>> Create(CategoryCreateDto dto)
    {
        try { return Ok(await _service.CreateAsync(dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{slug}")]
    public async Task<ActionResult<CategoryReadDto>> Update(string slug, CategoryCreateDto dto)
    {
        try { return Ok(await _service.UpdateAsync(slug, dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        try { await _service.DeleteAsync(slug); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
