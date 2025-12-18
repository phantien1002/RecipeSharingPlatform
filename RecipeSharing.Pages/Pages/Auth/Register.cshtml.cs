using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;

namespace RecipeSharing.Pages.Pages.Auth;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IAuthService _authService;

    public RegisterModel(IAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string FullName { get; set; } = string.Empty;

    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            Response.Redirect("/");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = new User
        {
            Email = Email,
            Password = Password,
            FullName = FullName
        };

        var success = await _authService.RegisterAsync(user);
        if (!success)
        {
            ModelState.AddModelError("", "Email already exists");
            return Page();
        }

        return RedirectToPage("/Auth/Login");
    }
}
