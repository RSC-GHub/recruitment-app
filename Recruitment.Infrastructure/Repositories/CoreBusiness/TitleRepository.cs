using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class TitleRepository : GenericRepository<Title>, ITitleRepository
    {

        public TitleRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<Title> GetByIdWithDepartmentsAsync(int id)
        {
            var entity = await _context.Titles
                .Include(t => t.DepartmentTitles!)
                    .ThenInclude(dt => dt.Department!)
                .FirstOrDefaultAsync(t => t.Id == id);

            return entity ?? throw new KeyNotFoundException($"Title with Id {id} not found");
        }
    }
}
