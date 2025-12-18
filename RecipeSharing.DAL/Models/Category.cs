using System;
using System.Collections.Generic;

namespace RecipeSharing.DAL.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    //public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
}
