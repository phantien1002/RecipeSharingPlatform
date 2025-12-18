using RecipeSharing.DAL.Models;

namespace RecipeSharing.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(User user);
    }
}
