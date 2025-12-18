using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.Services.Interfaces
{
    public interface IRecipeService
    {
        Recipe? GetRecipe(int recipeId);

        decimal CalculateQuantity(decimal baseQuantity, int baseServings, int selectedServings);
    }
}
