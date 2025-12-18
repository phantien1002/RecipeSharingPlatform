namespace RecipeSharing.BLL.ViewModels;

public class ShoppingListVm
{
    public int ShoppingListId { get; set; }
    public int UserId { get; set; }
    public DateTime? CreatedAt { get; set; }

    public List<ShoppingListItemVm> Items { get; set; } = new();
}

public class ShoppingListItemVm
{
    public int ItemId { get; set; }
    public int IngredientId { get; set; }
    public string IngredientName { get; set; } = "";
    public decimal Quantity { get; set; }
    public bool IsChecked { get; set; }
}
