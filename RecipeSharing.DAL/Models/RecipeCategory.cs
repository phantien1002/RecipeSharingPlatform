using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeSharing.DAL.Models
{
    public class RecipeCategory
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }

        public Recipe Recipe { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
