using LibraryFinalProject.src.Library.Api.Entities;

namespace LibraryFinalProject.src.Library.Api.Repositories;
public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}