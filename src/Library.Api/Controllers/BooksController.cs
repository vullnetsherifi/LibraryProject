using Library.Api.Dtos;
using Library.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly BookService _service;
    public BooksController(BookService service) => _service = service;

    // GET /api/books?q=harry&categoryId=...&availableOnly=true
    [HttpGet]
    public async Task<ActionResult<List<BookReadDto>>> Search(
        [FromQuery] string? q,
        [FromQuery] string? categorySlug,
        [FromQuery] bool? availableOnly)
        => Ok(await _service.SearchAsync(q, categorySlug, availableOnly));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookReadDto>> GetById(Guid id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<BookReadDto>> Create(BookCreateDto dto)
    {
        try { return Ok(await _service.CreateAsync(dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookReadDto>> Update(Guid id, BookCreateDto dto)
    {
        try { return Ok(await _service.UpdateAsync(id, dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try { await _service.DeleteAsync(id); return NoContent(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
