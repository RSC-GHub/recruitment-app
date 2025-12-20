using Recruitment.Application.Common;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface ITitleRepository
    {
        Task<PagedResult<Title>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        int? departmentId = null);

        Task<IEnumerable<Title>> GetAllWithDepartmentsAsync();
        Task<Title> GetByIdWithDepartmentsAsync(int id);
    }
}
