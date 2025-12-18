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
    public class MealPlanService : IMealPlanService
    {
        private readonly IMealPlanRepository _mealPlanRepository;
        public MealPlanService(IMealPlanRepository mealPlanRepository)
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public void CreateMealPlan(int userId, DateOnly date, string mealType, int recipeId)
        {
            var mealPlan = new MealPlan
            {
                UserId = userId,
                PlanDate = date,
                MealType = mealType,
                RecipeId = recipeId
            };

            _mealPlanRepository.Add(mealPlan);
        }

        public List<MealPlan> GetMealPlans(int userId, DateOnly date)
        {
            return _mealPlanRepository.GetByUserAndDate(userId, date);
        }
    }
}
