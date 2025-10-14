using LotoApp.Data;
using LotoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LotoApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByAuth0SubAsync(string auth0Sub)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Auth0Sub == auth0Sub);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
