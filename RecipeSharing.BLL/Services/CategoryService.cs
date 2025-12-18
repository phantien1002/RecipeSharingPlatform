using RecipeSharing.BLL.Interfaces;
using RecipeSharing.DAL.Models;
using RecipeSharing.DAL.Repositories;

namespace RecipeSharing.BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepo;

    public CategoryService(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepo.GetAllAsync();
    }
}
