using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class BookCreateDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Author { get; set; } = string.Empty;

    public int Year { get; set; }
    public bool IsAvailable { get; set; } = true;

    [Required, MaxLength(80)]
    public string CategorySlug { get; set; } = string.Empty;
}
