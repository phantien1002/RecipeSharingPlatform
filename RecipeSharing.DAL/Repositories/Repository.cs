using Microsoft.EntityFrameworkCore;
using RecipeSharing.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSharing.DAL.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly RecipeSharingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(RecipeSharingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
