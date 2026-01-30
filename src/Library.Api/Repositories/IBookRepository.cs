
using Library.Api.Entities;
using LibraryFinalProject.src.Library.Api.Entities;

namespace Library.Api.Repositories;

public interface IBookRepository
{
    Task<List<Book>> SearchAsync(string? q, string? categorySlug, bool? availableOnly);
    Task<Book?> GetByIdAsync(Guid id);

    // used by CategoryService delete check
    Task<bool> AnyBookInCategoryAsync(string categorySlug);

    Task AddAsync(Book book);
    void Remove(Book book);
    Task SaveChangesAsync();
}
