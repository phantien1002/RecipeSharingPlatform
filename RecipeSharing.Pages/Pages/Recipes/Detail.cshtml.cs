using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;

namespace RecipeSharing.Pages.Pages.Recipes
{
    public class DetailModel : PageModel
    {
        private readonly IRecipeService _recipeService;

        public DetailModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public Recipe Recipe { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            System.Diagnostics.Debug.WriteLine($"ID received: {id}");
            if (id <= 0) return Page();

            var recipe = await _recipeService.GetDetailAsync(id);
            if (recipe == null) return Page();

            Recipe = recipe;
            return Page();
        }
    }
}
