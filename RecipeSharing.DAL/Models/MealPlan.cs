using System;
using System.Collections.Generic;

namespace RecipeSharing.DAL.Models;

public partial class MealPlan
{
    public int MealPlanId { get; set; }

    public int UserId { get; set; }

    public DateOnly PlanDate { get; set; }

    public string? MealType { get; set; }

    public int RecipeId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
