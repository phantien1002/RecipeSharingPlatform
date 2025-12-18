using RecipeSharing.BLL.DTOs;
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
    public class RecipeService : IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepo;
        private readonly IRepository<RecipeIngredient> _recipeIngredientRepo;
        private readonly IRepository<RecipeCategory> _recipeCategoryRepo;

        public RecipeService(
            IRepository<Recipe> recipeRepo,
            IRepository<RecipeIngredient> recipeIngredientRepo,
            IRepository<RecipeCategory> recipeCategoryRepo)
        {
            _recipeRepo = recipeRepo;
            _recipeIngredientRepo = recipeIngredientRepo;
            _recipeCategoryRepo = recipeCategoryRepo;
        }

        public async Task CreateRecipeAsync(int userId, CreateRecipeDto dto)
        {
            var recipe = new Recipe
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                VideoUrl = dto.VideoUrl,
                ImageUrl = dto.ImageUrl,
                ServingSize = dto.ServingSize,
                CookTimeMinutes = dto.CookTimeMinutes,
                Difficulty = dto.Difficulty
            };

            foreach (var ing in dto.Ingredients)
            {
                await _recipeIngredientRepo.AddAsync(new RecipeIngredient
                {
                    Recipe = recipe,
                    IngredientId = ing.IngredientId,
                    Quantity = ing.Quantity
                });
            }

            foreach (var catId in dto.CategoryIds)
            {
                await _recipeCategoryRepo.AddAsync(new RecipeCategory
                {
                    Recipe = recipe,
                    CategoryId = catId
                });
            }

            await _recipeRepo.SaveChangesAsync();
        }

        public async Task<Recipe?> GetDetailAsync(int id)
        {
            return await _recipeRepo
                .Query()
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _recipeRepo.Query()
                .Include(r => r.RecipeCategories)
                    .ThenInclude(rc => rc.Category)
                .Include(r => r.User)
                .OrderByDescending(r => r.RecipeId)
                .ToListAsync();
        }

    }
}
