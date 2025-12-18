using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Services.Interfaces;
using RecipeSharing.DAL.Models;

namespace RecipeSharing.Pages.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly IRecipeService _recipeService;

        public DetailsModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public Recipe Recipe { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public int Servings { get; set; } 

        public IActionResult OnGet(int id, int? servings)
        {
            var recipe = _recipeService.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }

            Recipe = recipe;

            Servings = servings ?? recipe.ServingSize ?? 1;
            return Page();
        }

        public decimal GetCalculatedQuantity(decimal baseQuantity)
        {
            return _recipeService.CalculateQuantity(
                baseQuantity,
                Recipe.ServingSize ?? 1,
                Servings
        );
        }

    }
}
