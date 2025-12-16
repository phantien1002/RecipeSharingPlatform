using System;
using System.Collections.Generic;

namespace RecipeSharing.DAL.Models;

public partial class SavedRecipe
{
    public int SavedRecipeId { get; set; }

    public int UserId { get; set; }

    public int RecipeId { get; set; }

    public DateTime? SavedAt { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
