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
    public class MealPlanRepository : IMealPlanRepository
    {
        private readonly RecipeSharingDbContext _context;
        public MealPlanRepository(RecipeSharingDbContext context)
        {
            _context = context;
        }

        public void Add(MealPlan mealPlan)
        {
            _context.MealPlans.Add(mealPlan);
            _context.SaveChanges();
        }

        public List<MealPlan> GetByUserAndDate(int userId, DateOnly date)
        {
            return _context.MealPlans
                .Include(mp => mp.Recipe)
                .Where(mp => mp.UserId == userId && mp.PlanDate == date)
                .ToList();
        }
    }
}
