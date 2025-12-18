using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.DTOs;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.BLL.Services;
using RecipeSharing.DAL.Models;
using System.Security.Claims;
using System.Linq;

namespace RecipeSharing.Pages.Pages.Recipes;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IRecipeService _recipeService;
    private readonly IIngredientService _ingredientService;
    private readonly ICategoryService _categoryService;

    public CreateModel(
        IRecipeService recipeService,
        IIngredientService ingredientService,
        ICategoryService categoryService)
    {
        _recipeService = recipeService;
        _ingredientService = ingredientService;
        _categoryService = categoryService;
    }


    public IEnumerable<Category> AllCategories { get; set; } = Enumerable.Empty<Category>();
    public IEnumerable<Ingredient> AllIngredients { get; set; } = Enumerable.Empty<Ingredient>();

    public List<IngredientInputDto> Ingredients { get; set; } = new();


    [BindProperty]
    public CreateRecipeDto Recipe { get; set; } = new();

    [BindProperty]
    public List<int> SelectedCategoryIds { get; set; } = new();

    public async Task OnGetAsync()
    {
        AllCategories = await _categoryService.GetAllAsync();
        AllIngredients = await _ingredientService.GetAllAsync();


        Ingredients = new List<IngredientInputDto>
        {
            new(),
            new(),
            new()
        };

        Recipe.Ingredients = Ingredients;
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            AllCategories = await _categoryService.GetAllAsync();
            AllIngredients = await _ingredientService.GetAllAsync();
            if (Recipe.Ingredients == null || !Recipe.Ingredients.Any())
            {
                Recipe.Ingredients = new List<IngredientInputDto> { new(), new() };
            }
            return Page();
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Recipe.CategoryIds = SelectedCategoryIds;

        await _recipeService.CreateRecipeAsync(userId, Recipe);

        return RedirectToPage("/Index");
    }
}
