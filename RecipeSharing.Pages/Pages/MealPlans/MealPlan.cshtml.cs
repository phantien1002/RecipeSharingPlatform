using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Services.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories.Interfaces;

namespace RecipeSharing.Pages.Pages.MealPlans
{
    public class MealPlanModel : PageModel
    {
        private readonly IMealPlanService _mealPlanService;
        private readonly IRecipeRepository _recipeRepository;

        public MealPlanModel(IMealPlanService mealPlanService, IRecipeRepository recipeRepository)
        {
            _mealPlanService = mealPlanService;
            _recipeRepository = recipeRepository;
        }

        [BindProperty]
        public DateTime PlanDate { get; set; } = DateTime.Today;

        [BindProperty]
        public string MealType { get; set; } = null!;

        [BindProperty]
        public int RecipeId { get; set; }

        public List<Recipe> Recipes { get; set; } = new();

        public void OnGet()
        {
            Recipes = _recipeRepository.GetAll();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Recipes = _recipeRepository.GetAll();
                return Page();
            }

            _mealPlanService.CreateMealPlan(
                userId: 1,
                date: DateOnly.FromDateTime(PlanDate), 
                mealType: MealType,
                recipeId: RecipeId
            );

            return RedirectToPage("Index");
        }
    }
}
