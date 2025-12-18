using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;

namespace RecipeSharing.Pages.Pages.Recipes
{
    public class MySavedRecipesModel : PageModel
    {
        private readonly ISavedRecipeService _savedService;
        public MySavedRecipesModel(ISavedRecipeService savedService) => _savedService = savedService;

        public IEnumerable<Recipe> SavedRecipes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Sử dụng ClaimTypes.NameIdentifier để khớp với code Login của bạn
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Auth/Login");

            // Dùng chính hàm trong Service của bạn
            SavedRecipes = await _savedService.GetSavedRecipesByUserIdAsync(int.Parse(userIdStr));
            return Page();
        }
    }
}