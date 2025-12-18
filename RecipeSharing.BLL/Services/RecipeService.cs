using RecipeSharing.BLL.Services.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public Recipe? GetRecipe(int recipeId)
        {
            return _recipeRepository.GetRecipeWithIngredients(recipeId);
        }

        public decimal CalculateQuantity(decimal baseQuantity, int baseServings, int selectedServings)
        {
            if (baseServings <= 0) return baseQuantity;

            return (baseQuantity * selectedServings) / baseServings;
        }
    }
}
