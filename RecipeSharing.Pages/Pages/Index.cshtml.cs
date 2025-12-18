using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;

namespace RecipeSharing.Pages.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly IRecipeService _recipeService;
        private readonly ISavedRecipeService _savedService;

        public IndexModel(IRecipeService recipeService, ISavedRecipeService savedService)
        {
            _recipeService = recipeService;
            _savedService = savedService;
        }

        public IEnumerable<Recipe> Recipes { get; set; } = new List<Recipe>();

        public List<int> SavedRecipeIds { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            Recipes = await _recipeService.GetAllAsync();

            var userIdStr = User.FindFirst("UserId")?.Value;

            if (!string.IsNullOrEmpty(userIdStr))
            {
                int userId = int.Parse(userIdStr);
                var savedRecipes = await _savedService.GetSavedRecipesByUserIdAsync(userId);
                SavedRecipeIds = savedRecipes.Select(r => r.RecipeId).ToList();
            }
        }
    }
}