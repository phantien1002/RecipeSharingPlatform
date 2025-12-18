using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.DAL.Repositories.Interfaces
{
    public interface IMealPlanRepository
    {
        void Add(MealPlan mealPlan);
        List<MealPlan> GetByUserAndDate(int userId, DateOnly date);
    }
}
