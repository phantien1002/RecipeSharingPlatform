using Microsoft.EntityFrameworkCore;
using RecipeSharing.BLL.Services;
using RecipeSharing.BLL.Services.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories;
using RecipeSharing.DAL.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<RecipeSharingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeSharingDbConnection")));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();

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
