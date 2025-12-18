using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.Services.Interfaces
{
    public interface IMealPlanService
    {
        void CreateMealPlan(int userId, DateOnly date, string mealType, int recipeId);

        List<MealPlan> GetMealPlans(int userId, DateOnly date);
    }
}
