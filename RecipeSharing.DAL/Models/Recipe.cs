using System;
using System.Collections.Generic;

namespace RecipeSharing.DAL.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? VideoUrl { get; set; }

    public string? ImageUrl { get; set; }

    public int? ServingSize { get; set; }

    public int? CookTimeMinutes { get; set; }

    public string? Difficulty { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<SavedRecipe> SavedRecipes { get; set; } = new List<SavedRecipe>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
