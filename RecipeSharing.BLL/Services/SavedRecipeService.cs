using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace RecipeSharing.BLL.Services
{
    public class SavedRecipeService : ISavedRecipeService
    {
        private readonly IRepository<SavedRecipe> _savedRepo;

        public SavedRecipeService(IRepository<SavedRecipe> savedRepo)
        {
            _savedRepo = savedRepo;
        }


        public async Task<string> ToggleSaveRecipeAsync(int userId, int recipeId)
        {
            var existing = await _savedRepo.Query()
                .Include(sr => sr.Recipe)
                .FirstOrDefaultAsync(sr => sr.UserId == userId && sr.RecipeId == recipeId);

            string recipeName = "";

            if (existing != null)
            {
                recipeName = existing.Recipe?.Title ?? "Món ăn";
                _savedRepo.Delete(existing);
            }
            else
            {

                await _savedRepo.AddAsync(new SavedRecipe
                {
                    UserId = userId,
                    RecipeId = recipeId,
                    SavedAt = DateTime.Now
                });

                var recipe = await _savedRepo.Query()
                    .Where(x => x.RecipeId == recipeId)
                    .Select(x => x.Recipe.Title)
                    .FirstOrDefaultAsync();
                recipeName = recipe ?? "Món ăn";
            }

            await _savedRepo.SaveChangesAsync();
            return recipeName;
        }

        public async Task<bool> IsRecipeSavedAsync(int userId, int recipeId)
        {
            return await _savedRepo.Query()
                .AnyAsync(sr => sr.UserId == userId && sr.RecipeId == recipeId);
        }
        public async Task<List<int>> GetSavedRecipeIdsAsync(int userId)
        {
            return await _savedRepo.Query()
                .Where(sr => sr.UserId == userId)
                .Select(sr => sr.RecipeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetSavedRecipesByUserIdAsync(int userId)
        {
            return await _savedRepo.Query()
                .Where(sr => sr.UserId == userId)
                .Include(sr => sr.Recipe)
                    .ThenInclude(r => r.RecipeCategories)
                        .ThenInclude(rc => rc.Category)
                .Select(sr => sr.Recipe)
                .ToListAsync();
        }
    }
}
