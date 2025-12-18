using Recruitment.Application.Common;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface IDepartmentRepository
    {
        Task<PagedResult<Department>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null);
    }

}
