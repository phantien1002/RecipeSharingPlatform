using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.DAL.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Recipe? GetRecipeWithIngredients(int recipeId);
    }
}
