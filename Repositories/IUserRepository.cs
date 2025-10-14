using LotoApp.Models;

namespace LotoApp.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByAuth0SubAsync(string auth0Sub);
        Task<User> AddAsync(User user);
        Task SaveChangesAsync();
    }
}
