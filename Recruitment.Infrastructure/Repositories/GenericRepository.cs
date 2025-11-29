using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Domain.Entities;
using Recruitment.Infrastructure.Data;
using System.Linq.Expressions;

namespace Recruitment.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                baseEntity.ModifiedOn = DateTime.UtcNow;
                baseEntity.ModifiedBy = GetCurrentUsername();
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }
    }
}