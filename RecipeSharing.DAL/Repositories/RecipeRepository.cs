using Microsoft.EntityFrameworkCore;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.DAL.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeSharingDbContext _context;
        public RecipeRepository(RecipeSharingDbContext context)
        {
            _context = context;
        }

        public Recipe? GetRecipeWithIngredients(int recipeId)
        {
            return _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefault(r => r.RecipeId == recipeId);
        }

        public List<Recipe> GetAll()
        {
            return _context.Recipes.ToList();
        }
    }
}
