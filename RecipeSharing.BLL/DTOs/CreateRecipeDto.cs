using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.BLL.DTOs { 
public class CreateRecipeDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }

    public int? ServingSize { get; set; }
    public int? CookTimeMinutes { get; set; }
    public string? Difficulty { get; set; }

    public List<int> CategoryIds { get; set; } = new();
    public List<IngredientInputDto> Ingredients { get; set; } = new();
}


}
