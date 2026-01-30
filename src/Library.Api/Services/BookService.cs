using Library.Api.Dtos;
using Library.Api.Entities;
using Library.Api.Repositories;

namespace Library.Api.Services;

public class BookService
{
    private readonly IBookRepository _books;
    private readonly ICategoryRepository _categories;

    public BookService(IBookRepository books, ICategoryRepository categories)
    {
        _books = books;
        _categories = categories;
    }

    public async Task<List<BookReadDto>> SearchAsync(string? q, string? categorySlug, bool? availableOnly)
    {
        var items = await _books.SearchAsync(q, categorySlug, availableOnly);
        return items.Select(ToDto).ToList();
    }

    public async Task<BookReadDto?> GetByIdAsync(Guid id)
    {
        var b = await _books.GetByIdAsync(id);
        return b == null ? null : ToDto(b);
    }

    public async Task<BookReadDto> CreateAsync(BookCreateDto dto)
    {
        ValidateYear(dto.Year);

        var slug = dto.CategorySlug.Trim();
        var category = await _categories.GetBySlugAsync(slug);
        if (category == null) throw new InvalidOperationException("Category not found.");

        var book = new Book
        {
            Title = dto.Title.Trim(),
            Author = dto.Author.Trim(),
            Year = dto.Year,
            IsAvailable = dto.IsAvailable,
            CategorySlug = slug
        };

        await _books.AddAsync(book);
        await _books.SaveChangesAsync();

        // reload with category
        var created = await _books.GetByIdAsync(book.Id);
        return ToDto(created!);
    }

    public async Task<BookReadDto> UpdateAsync(Guid id, BookCreateDto dto)
    {
        ValidateYear(dto.Year);

        var book = await _books.GetByIdAsync(id);
        if (book == null) throw new InvalidOperationException("Book not found.");

        var slug = dto.CategorySlug.Trim();
        var category = await _categories.GetBySlugAsync(slug);
        if (category == null) throw new InvalidOperationException("Category not found.");

        book.Title = dto.Title.Trim();
        book.Author = dto.Author.Trim();
        book.Year = dto.Year;
        book.IsAvailable = dto.IsAvailable;
        book.CategorySlug = slug;

        await _books.SaveChangesAsync();

        var updated = await _books.GetByIdAsync(book.Id);
        return ToDto(updated!);
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _books.GetByIdAsync(id);
        if (book == null) throw new InvalidOperationException("Book not found.");

        _books.Remove(book);
        await _books.SaveChangesAsync();
    }

    private static void ValidateYear(int year)
    {
        var max = DateTime.UtcNow.Year + 1;
        if (year != 0 && (year < 1400 || year > max))
            throw new InvalidOperationException($"Year must be between 1400 and {max}, or 0.");
    }

    private static BookReadDto ToDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        Author = b.Author,
        Year = b.Year,
        IsAvailable = b.IsAvailable,
        CategorySlug = b.CategorySlug,
        CategoryName = b.Category?.Name ?? ""
    };
}
