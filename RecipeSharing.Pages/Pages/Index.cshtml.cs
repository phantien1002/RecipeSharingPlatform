using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                SavedRecipeIds = await _savedService.GetSavedRecipeIdsAsync(int.Parse(userIdStr));
            }
        }

        public async Task<IActionResult> OnPostSaveAsync(int recipeId)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;

            // Kiểm tra log xem UserId có lấy được không
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Auth/Login");

            int userId = int.Parse(userIdStr);
            await _savedService.ToggleSaveRecipeAsync(userId, recipeId);

            // Dùng đường dẫn đầy đủ tính từ thư mục Pages
            return RedirectToPage("/Pages/Recipes/MySavedRecipes");
        }
    }
}