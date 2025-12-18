using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.Interfaces
{
    public interface ISavedRecipeService
    {
        Task<string> ToggleSaveRecipeAsync(int userId, int recipeId);
        Task<bool> IsRecipeSavedAsync(int userId, int recipeId);
        Task<List<int>> GetSavedRecipeIdsAsync(int userId);
        Task<IEnumerable<Recipe>> GetSavedRecipesByUserIdAsync(int userId);
    }
}
