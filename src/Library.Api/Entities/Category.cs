using LibraryFinalProject.src.Library.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Api.Entities;

public class Category
{
    [Key]
    [MaxLength(80)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = new();
}
