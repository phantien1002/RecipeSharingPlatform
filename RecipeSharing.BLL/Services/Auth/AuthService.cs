using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories;

namespace RecipeSharing.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepo;

        public AuthService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var users = await _userRepo.GetAllAsync();

            return users.FirstOrDefault(u =>
                u.Email == email && u.Password == password);
        }

        public async Task<bool> RegisterAsync(User user)
        {
            var users = await _userRepo.GetAllAsync();

            if (users.Any(u => u.Email == user.Email))
                return false;

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return true;
        }
    }
}
