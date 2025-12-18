using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using RecipeSharing.BLL.Interfaces; 
using RecipeSharing.Pages.Hubs;

namespace RecipeSharing.Pages.Pages.Recipes
{
    public class SaveActionModel : PageModel
    {
        private readonly ISavedRecipeService _savedService;
        private readonly IHubContext<RecipeHub> _hubContext;

        public SaveActionModel(ISavedRecipeService savedService, IHubContext<RecipeHub> hubContext)
        {
            _savedService = savedService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnPostAsync(int recipeId, string returnUrl)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Auth/Login");

            int userId = int.Parse(userIdStr);

            // Hứng lấy tên món ăn từ Service trả về
            string recipeName = await _savedService.ToggleSaveRecipeAsync(userId, recipeId);

            // Bắn qua SignalR
            await _hubContext.Clients.All.SendAsync("UpdateSaveStatus", recipeId, recipeName);

            if (!string.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);
            return RedirectToPage("./Index");
        }
    }
}