using Library.Api.Data;
using Library.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;
    public CategoryRepository(AppDbContext db) => _db = db;

    public Task<List<Category>> GetAllAsync()
        => _db.Categories.OrderBy(c => c.Name).ToListAsync();

    public Task<Category?> GetBySlugAsync(string slug)
        => _db.Categories.FirstOrDefaultAsync(c => c.Slug == slug);

    public async Task AddAsync(Category category)
        => await _db.Categories.AddAsync(category);

    public void Remove(Category category)
        => _db.Categories.Remove(category);

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
