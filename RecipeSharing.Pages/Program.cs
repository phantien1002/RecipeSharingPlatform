using Microsoft.EntityFrameworkCore;
using RecipeSharing.DAL.Models;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();


builder.Services.AddDbContext<RecipeSharingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
