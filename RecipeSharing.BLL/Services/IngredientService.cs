using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IRepository<Ingredient> _ingredientRepo;

        public IngredientService(IRepository<Ingredient> ingredientRepo)
        {
            _ingredientRepo = ingredientRepo;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _ingredientRepo.GetAllAsync();
        }
    }
}
