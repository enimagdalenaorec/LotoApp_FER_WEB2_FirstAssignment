using LotoApp.Models;
using LotoApp.Repositories;

namespace LotoApp.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetOrCreateUserAsync(string auth0Sub, string? email)
        {
            var existingUser = await _userRepository.GetByAuth0SubAsync(auth0Sub);

            if (existingUser != null)
                return existingUser;

            var newUser = new User
            {
                Auth0Sub = auth0Sub,
                Email = email
            };

            return await _userRepository.AddAsync(newUser);
        }
    }
}
