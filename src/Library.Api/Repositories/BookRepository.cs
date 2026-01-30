using Library.Api.Data;
using Library.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;
    public BookRepository(AppDbContext db) => _db = db;

    public async Task<List<Book>> SearchAsync(string? q, string? categorySlug, bool? availableOnly)
    {
        var query = _db.Books.Include(b => b.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim().ToLower();
            query = query.Where(b =>
                b.Title.ToLower().Contains(term) ||
                b.Author.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var slug = categorySlug.Trim().ToLower();
            query = query.Where(b => b.CategorySlug == slug);
        }

        if (availableOnly == true)
            query = query.Where(b => b.IsAvailable);

        return await query.OrderBy(b => b.Title).ToListAsync();
    }

    public Task<Book?> GetByIdAsync(Guid id)
        => _db.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id);

    public Task<bool> AnyBookInCategoryAsync(string categorySlug)
        => _db.Books.AnyAsync(b => b.CategorySlug == categorySlug);

    public async Task AddAsync(Book book)
        => await _db.Books.AddAsync(book);

    public void Remove(Book book)
        => _db.Books.Remove(book);

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
