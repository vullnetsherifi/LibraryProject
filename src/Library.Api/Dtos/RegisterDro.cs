using System.ComponentModel.DataAnnotations;

namespace LibraryFinalProject.src.Library.Api.Dtos;
public class RegisterDto
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    // optional: if empty -> "Member"
    public string? Role { get; set; }
}