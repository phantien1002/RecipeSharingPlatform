using Microsoft.EntityFrameworkCore;
using RecipeSharing.BLL.Interfaces;
using RecipeSharing.BLL.ViewModels;
using RecipeSharing.DAL;
using RecipeSharing.DAL.Models;

namespace RecipeSharing.BLL.Services;

public class ShoppingListService : IShoppingListService
{
    private readonly RecipeSharingDbContext _db;

    public ShoppingListService(RecipeSharingDbContext db)
    {
        _db = db;
    }

    // Generate: MealPlans -> RecipeIngredients -> group Ingredient -> ShoppingListItems
    // Vì ShoppingList không có From/To trong DB, mình tạo 1 list mới mỗi lần Generate (đúng “flow” và an toàn).
    //public async Task<int> GenerateAsync(int userId, DateOnly fromDate, DateOnly toDate)
    //{
    //    if (fromDate > toDate) throw new ArgumentException("Invalid date range.");

    //    using var tx = await _db.Database.BeginTransactionAsync();

    //    // 1) Pull mealplans in range (server query đơn giản)
    //    var mealPlans = await _db.MealPlans
    //        .Where(mp => mp.UserId == userId && mp.PlanDate >= fromDate && mp.PlanDate <= toDate)
    //        .Select(mp => mp.RecipeId)
    //        .ToListAsync();


    //    // 2) Tạo ShoppingList header như cũ...
    //    var list = new ShoppingList
    //    {
    //        UserId = userId,
    //        CreatedAt = DateTime.UtcNow
    //    };
    //    _db.ShoppingLists.Add(list);
    //    await _db.SaveChangesAsync();


    //    // 3) nếu rỗng thì return
    //    if (mealPlans.Count == 0)
    //    {
    //        await tx.CommitAsync();
    //        return list.ShoppingListId;
    //    }

    //    // 4) JOIN MealPlans -> RecipeIngredients (không Contains)
    //    var raw = await (
    //        from mp in _db.MealPlans
    //        join ri in _db.RecipeIngredients on mp.RecipeId equals ri.RecipeId
    //        where mp.UserId == userId && mp.PlanDate >= fromDate && mp.PlanDate <= toDate
    //        select new { ri.IngredientId, ri.Quantity }
    //    ).ToListAsync();

    //    // 5) Group in C#
    //    var aggregated = raw
    //        .GroupBy(x => x.IngredientId)
    //        .Select(g => new { IngredientId = g.Key, TotalQty = g.Sum(x => x.Quantity) })
    //        .ToList();

    //    // 6) insert ShoppingListItems
    //    var items = aggregated.Select(a => new ShoppingListItem
    //    {
    //        ShoppingListId = list.ShoppingListId,
    //        IngredientId = a.IngredientId,
    //        Quantity = a.TotalQty,       // decimal? => gán decimal ok
    //        IsChecked = false
    //    });

    //    _db.ShoppingListItems.AddRange(items);
    //    await _db.SaveChangesAsync();

    //    await tx.CommitAsync();
    //    return list.ShoppingListId;
    //}
    public async Task<int> GenerateAsync(int userId, DateOnly fromDate, DateOnly toDate)
    {
        if (fromDate > toDate) throw new ArgumentException("Invalid date range.");

        // Validate user exists (tránh lỗi FK)
        var userExists = await _db.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists) throw new Exception($"UserId {userId} không tồn tại.");

        // 1) Lấy nguyên liệu từ MealPlans + RecipeIngredients (không dùng Contains để né lỗi SQL WITH)
        var raw = await (
            from mp in _db.MealPlans
            join ri in _db.RecipeIngredients on mp.RecipeId equals ri.RecipeId
            where mp.UserId == userId
               && mp.PlanDate >= fromDate
               && mp.PlanDate <= toDate
            select new { ri.IngredientId, ri.Quantity }
        ).ToListAsync();

        // 2) Aggregate trong C# (gộp ingredient trùng)
        var aggregated = raw
            .GroupBy(x => x.IngredientId)
            .Select(g => new
            {
                IngredientId = g.Key,
                TotalQty = g.Sum(x => x.Quantity)
            })
            .ToList();

        // 3) Lấy ShoppingList mới nhất của user để UPSERT
        var list = await _db.ShoppingLists
            .Include(l => l.ShoppingListItems)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync();

        // Nếu chưa có list nào thì tạo list đầu tiên
        if (list == null)
        {
            list = new ShoppingList
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _db.ShoppingLists.Add(list);
            await _db.SaveChangesAsync();

            // đảm bảo navigation không null
            list.ShoppingListItems = new List<ShoppingListItem>();
        }

        // 4) Map item hiện có theo IngredientId để giữ IsChecked
        var existingByIngredient = list.ShoppingListItems
            .ToDictionary(x => x.IngredientId, x => x);

        var newIngredientIds = aggregated.Select(a => a.IngredientId).ToHashSet();

        // 5) UPSERT items
        foreach (var a in aggregated)
        {
            if (existingByIngredient.TryGetValue(a.IngredientId, out var exist))
            {
                // Update quantity, giữ nguyên IsChecked
                exist.Quantity = a.TotalQty;
            }
            else
            {
                // Add mới
                list.ShoppingListItems.Add(new ShoppingListItem
                {
                    ShoppingListId = list.ShoppingListId,
                    IngredientId = a.IngredientId,
                    Quantity = a.TotalQty,
                    IsChecked = false
                });
            }
        }

        // 6) Remove items không còn trong aggregated (nếu MealPlans đổi)
        var removeItems = list.ShoppingListItems
            .Where(x => !newIngredientIds.Contains(x.IngredientId))
            .ToList();

        if (removeItems.Count > 0)
            _db.ShoppingListItems.RemoveRange(removeItems);

        // Cập nhật thời gian để thể hiện “Generate lại”
        list.CreatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return list.ShoppingListId;
    }


    public async Task<ShoppingListVm?> GetAsync(int userId, int shoppingListId)
    {
        var list = await _db.ShoppingLists
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShoppingListId == shoppingListId && x.UserId == userId);

        if (list == null) return null;

        // Ingredient.Name có thể khác tên trong project bạn (nếu khác, sửa lại chỗ ing.Name)
        var items = await _db.ShoppingListItems
            .Where(i => i.ShoppingListId == shoppingListId)
            .Join(_db.Ingredients,
                  i => i.IngredientId,
                  ing => ing.IngredientId,
                  (i, ing) => new ShoppingListItemVm
                  {
                      ItemId = i.ItemId,
                      IngredientId = i.IngredientId,
                      IngredientName = ing.Name,                 // nếu entity Ingredient không phải Name -> sửa
                      Quantity = i.Quantity ?? 0,
                      IsChecked = i.IsChecked ?? false
                  })
            .OrderBy(x => x.IngredientName)
            .ToListAsync();

        return new ShoppingListVm
        {
            ShoppingListId = list.ShoppingListId,
            UserId = list.UserId,
            CreatedAt = list.CreatedAt,
            Items = items
        };
    }
    public async Task<ShoppingListVm?> GetLatestAsync(int userId)
    {
        var latestId = await _db.ShoppingLists
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (int?)x.ShoppingListId)
            .FirstOrDefaultAsync();

        if (!latestId.HasValue) return null;

        return await GetAsync(userId, latestId.Value);
    }
    public async Task ToggleCheckedAsync(int userId, int itemId, bool isChecked)
    {
        var item = await _db.ShoppingListItems
            .Include(i => i.ShoppingList)
            .FirstOrDefaultAsync(i => i.ItemId == itemId);

        if (item == null) throw new KeyNotFoundException("Item not found");
        if (item.ShoppingList.UserId != userId) throw new UnauthorizedAccessException();

        item.IsChecked = isChecked;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int userId, int itemId, decimal quantity)
    {
        if (quantity < 0) throw new ArgumentException("Quantity must be >= 0");

        var item = await _db.ShoppingListItems
            .Include(i => i.ShoppingList)
            .FirstOrDefaultAsync(i => i.ItemId == itemId);

        if (item == null) throw new KeyNotFoundException("Item not found");
        if (item.ShoppingList.UserId != userId) throw new UnauthorizedAccessException();

        item.Quantity = quantity;
        await _db.SaveChangesAsync();
    }
}
