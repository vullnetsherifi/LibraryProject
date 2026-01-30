using Library.Api.Data;
using LibraryFinalProject.src.Library.Api.Entities;
using Microsoft.EntityFrameworkCore;



namespace LibraryFinalProject.src.Library.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByUsernameAsync(string username)
        => _db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user)
        => await _db.Users.AddAsync(user);

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
