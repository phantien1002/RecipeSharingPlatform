using RecipeSharing.BLL.DTOs;
using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.Interfaces
{
    public interface IRecipeService
    {
        Task CreateRecipeAsync(int userId, CreateRecipeDto dto);
        Task<Recipe?> GetDetailAsync(int id);
        Task<IEnumerable<Recipe>> GetAllAsync();

    }

}
