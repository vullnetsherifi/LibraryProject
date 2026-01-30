using Library.Api.Dtos;
using Library.Api.Entities;
using Library.Api.ErrorHandling;
using Library.Api.Repositories;

namespace Library.Api.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categories;
    private readonly IBookRepository _books;

    public CategoryService(ICategoryRepository categories, IBookRepository books)
    {
        _categories = categories;
        _books = books;
    }

    public async Task<List<CategoryReadDto>> GetAllAsync()
    {
        var items = await _categories.GetAllAsync();
        return items.Select(c => new CategoryReadDto
        {
            Slug = c.Slug,
            Name = c.Name
        }).ToList();
    }

    public async Task<CategoryReadDto?> GetBySlugAsync(string slug)
    {
        slug = slug.Trim().ToLower();
        var c = await _categories.GetBySlugAsync(slug);

        return c == null ? null : new CategoryReadDto
        {
            Slug = c.Slug,
            Name = c.Name
        };
    }

    public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
    {
        var name = dto.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Name is required.");

        var slug = SlugHelper.Generate(name);

        var existing = await _categories.GetBySlugAsync(slug);
        if (existing != null)
            throw new InvalidOperationException("Category already exists.");

        var category = new Category
        {
            Name = name,
            Slug = slug
        };

        await _categories.AddAsync(category);
        await _categories.SaveChangesAsync();

        return new CategoryReadDto { Slug = category.Slug, Name = category.Name };
    }

    public async Task<CategoryReadDto> UpdateAsync(string slug, CategoryCreateDto dto)
    {
        slug = slug.Trim().ToLower();
        var category = await _categories.GetBySlugAsync(slug);
        if (category == null)
            throw new InvalidOperationException("Category not found.");

        var name = dto.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("Name is required.");

        var newSlug = SlugHelper.Generate(name);

        var conflict = await _categories.GetBySlugAsync(newSlug);
        if (conflict != null && conflict.Slug != category.Slug)
            throw new InvalidOperationException("Category already exists.");

        // IMPORTANT: if you want slug immutable, remove the next line and only update Name.
        category.Name = name;
        category.Slug = newSlug;

        await _categories.SaveChangesAsync();

        return new CategoryReadDto { Slug = category.Slug, Name = category.Name };
    }

    public async Task DeleteAsync(string slug)
    {
        slug = slug.Trim().ToLower();
        var category = await _categories.GetBySlugAsync(slug);
        if (category == null)
            throw new InvalidOperationException("Category not found.");

        var hasBooks = await _books.AnyBookInCategoryAsync(category.Slug);
        if (hasBooks)
            throw new InvalidOperationException("Cannot delete category with books.");

        _categories.Remove(category);
        await _categories.SaveChangesAsync();
    }
}
