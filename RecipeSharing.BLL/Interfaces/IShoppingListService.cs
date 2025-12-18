using RecipeSharing.BLL.ViewModels;

namespace RecipeSharing.BLL.Interfaces;

public interface IShoppingListService
{
    Task<int> GenerateAsync(int userId, DateOnly fromDate, DateOnly toDate);

    Task<ShoppingListVm?> GetAsync(int userId, int shoppingListId);

    Task ToggleCheckedAsync(int userId, int itemId, bool isChecked);

    Task UpdateQuantityAsync(int userId, int itemId, decimal quantity);

    Task<ShoppingListVm?> GetLatestAsync(int userId);

}
