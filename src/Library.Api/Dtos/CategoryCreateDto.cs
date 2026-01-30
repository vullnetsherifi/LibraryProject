using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class CategoryCreateDto
{
    [Required, MaxLength(60)]
    public string Name { get; set; } = string.Empty;
}
