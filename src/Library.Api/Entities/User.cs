using System.ComponentModel.DataAnnotations;

namespace LibraryFinalProject.src.Library.Api.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // "Admin" or "Member"
    [Required, MaxLength(20)]
    public string Role { get; set; } = "Member";
}
