using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipeSharing.DAL.Models;

public partial class RecipeSharingDbContext : DbContext
{
    public RecipeSharingDbContext()
    {
    }

    public RecipeSharingDbContext(DbContextOptions<RecipeSharingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<MealPlan> MealPlans { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeCategory> RecipeCategories { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<SavedRecipe> SavedRecipes { get; set; }

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeCategory>()
        .HasKey(rc => new { rc.RecipeId, rc.CategoryId });

        modelBuilder.Entity<RecipeCategory>()
            .HasOne(rc => rc.Recipe)
            .WithMany(r => r.RecipeCategories)
            .HasForeignKey(rc => rc.RecipeId);

        modelBuilder.Entity<RecipeCategory>()
            .HasOne(rc => rc.Category)
            .WithMany(c => c.RecipeCategories)
            .HasForeignKey(rc => rc.CategoryId);
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BBD597078");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("PK__Ingredie__BEAEB25A585CD7DD");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.MealPlanId).HasName("PK__MealPlan__0620DB76F32FB9FA");

            entity.Property(e => e.MealType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Recipe).WithMany(p => p.MealPlans)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MealPlans__Recip__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.MealPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MealPlans__UserI__45F365D3");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipes__FDD988B019ED8F08");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Difficulty)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServingSize).HasDefaultValue(1);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipes__UserId__3D5E1FD2");

            //entity.HasMany(d => d.Categories).WithMany(p => p.Recipes)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "RecipeCategory",
            //        r => r.HasOne<Category>().WithMany()
            //            .HasForeignKey("CategoryId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__RecipeCat__Categ__59FA5E80"),
            //        l => l.HasOne<Recipe>().WithMany()
            //            .HasForeignKey("RecipeId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__RecipeCat__Recip__59063A47"),
            //        j =>
            //        {
            //            j.HasKey("RecipeId", "CategoryId").HasName("PK__RecipeCa__5C491B10960ADF31");
            //            j.ToTable("RecipeCategories");
            //        });
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.RecipeIngredientId).HasName("PK__RecipeIn__A2C34216EC32FDC1");

            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeIng__Ingre__4316F928");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecipeIng__Recip__4222D4EF");
        });

        modelBuilder.Entity<SavedRecipe>(entity =>
        {
            entity.HasKey(e => e.SavedRecipeId).HasName("PK__SavedRec__AEEB3B693CEA3907");

            entity.Property(e => e.SavedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Recipe).WithMany(p => p.SavedRecipes)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SavedReci__Recip__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.SavedRecipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SavedReci__UserI__534D60F1");
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.HasKey(e => e.ShoppingListId).HasName("PK__Shopping__6CBBDD14732D38B0");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingL__UserI__4AB81AF0");
        });

        modelBuilder.Entity<ShoppingListItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Shopping__727E838BA08B99ED");

            entity.Property(e => e.IsChecked).HasDefaultValue(false);
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ShoppingListItems)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingL__Ingre__4F7CD00D");

            entity.HasOne(d => d.ShoppingList).WithMany(p => p.ShoppingListItems)
                .HasForeignKey(d => d.ShoppingListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShoppingL__Shopp__4E88ABD4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C0EAB6E48");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342A0F3682").IsUnique();

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
