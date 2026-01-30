
using Library.Api.Entities;
using LibraryFinalProject.src.Library.Api.Entities;

namespace Library.Api.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetBySlugAsync(string slug);
    Task AddAsync(Category category);
    void Remove(Category category);
    Task SaveChangesAsync();

}
