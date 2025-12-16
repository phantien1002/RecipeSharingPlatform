using Microsoft.EntityFrameworkCore;
using RecipeSharing.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<RecipeSharingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RecipeSharingDbConnection")));

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
