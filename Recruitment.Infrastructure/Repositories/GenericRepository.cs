using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Domain.Entities;
using Recruitment.Infrastructure.Data;
using System.Linq.Expressions;

namespace Recruitment.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
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
            => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);


        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).AsNoTracking().ToListAsync();

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

    }

}