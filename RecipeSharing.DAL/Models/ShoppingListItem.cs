using System;
using System.Collections.Generic;

namespace RecipeSharing.DAL.Models;

public partial class ShoppingListItem
{
    public int ItemId { get; set; }

    public int ShoppingListId { get; set; }

    public int IngredientId { get; set; }

    public decimal? Quantity { get; set; }

    public bool? IsChecked { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual ShoppingList ShoppingList { get; set; } = null!;
}
