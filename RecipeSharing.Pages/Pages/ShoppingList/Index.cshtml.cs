using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.BLL.ViewModels;

namespace RecipeSharing.Pages.Pages.ShoppingList;

public class IndexModel : PageModel
{
    private readonly IShoppingListService _service;

    public IndexModel(IShoppingListService service)
    {
        _service = service;
    }

    // demo: lấy userId từ query (sau này thay bằng login claims)
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int? ShoppingListId { get; set; }

    [BindProperty]
    public DateOnly From { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(-7));

    [BindProperty]
    public DateOnly To { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public ShoppingListVm? ListVm { get; set; }

    public async Task OnGetAsync()
    {
        // Chỉ hiển thị list khi đã generate và có ShoppingListId trên query
        if (ShoppingListId.HasValue)
        {
            ListVm = await _service.GetAsync(UserId, ShoppingListId.Value);
        }
        else
        {
            ListVm = null; // vào trang lần đầu không hiện list
        }
    }


    public async Task<IActionResult> OnPostGenerateAsync()
    {
        // Generate xong redirect về page với ShoppingListId
        var listId = await _service.GenerateAsync(UserId, From, To);
        return RedirectToPage(new { UserId, ShoppingListId = listId });
    }

    public async Task<IActionResult> OnPostToggleCheckedAsync([FromBody] ToggleCheckedRequest req)
    {
        await _service.ToggleCheckedAsync(UserId, req.ItemId, req.IsChecked);
        return new JsonResult(new { success = true });
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync([FromBody] UpdateQuantityRequest req)
    {
        await _service.UpdateQuantityAsync(UserId, req.ItemId, req.Quantity);
        return new JsonResult(new { success = true });
    }

    public record ToggleCheckedRequest(int ItemId, bool IsChecked);
    public record UpdateQuantityRequest(int ItemId, decimal Quantity);
}
