using LibraryFinalProject.src.Library.Api.Auth;
using LibraryFinalProject.src.Library.Api.Dtos;
using LibraryFinalProject.src.Library.Api.Entities;
using LibraryFinalProject.src.Library.Api.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LibraryFinalProject.src.Library.Api.Services;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly JwtTokenService _jwt;
    private readonly PasswordHasher<User> _hasher;

    public AuthService(IUserRepository users, JwtTokenService jwt, PasswordHasher<User> hasher)
    {
        _users = users;
        _jwt = jwt;
        _hasher = hasher;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existing = await _users.GetByUsernameAsync(dto.Username.Trim());
        if (existing != null)
            throw new InvalidOperationException("Username already exists.");

        var role = string.IsNullOrWhiteSpace(dto.Role) ? "Member" : dto.Role.Trim();
        if (role != "Admin" && role != "Member")
            throw new InvalidOperationException("Role must be Admin or Member.");

        var user = new User
        {
            Username = dto.Username.Trim(),
            Role = role
        };

        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return new AuthResponseDto
        {
            Username = user.Username,
            Role = user.Role,
            Token = _jwt.CreateToken(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _users.GetByUsernameAsync(dto.Username.Trim());
        if (user == null)
            throw new InvalidOperationException("Invalid credentials.");

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("Invalid credentials.");

        return new AuthResponseDto
        {
            Username = user.Username,
            Role = user.Role,
            Token = _jwt.CreateToken(user)
        };
    }
}