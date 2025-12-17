using Recruitment.Application.Common;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<PagedResult<Project>> GetPagedAsync(
           int page,
           int pageSize,
           string? search = null,
           int? countryId = null);
        Task<Project?> GetProjectWithVacanciesAsync(int projectId);
        Task<Project?> GetProjectWithLocationAsync(int projectId);

        Task<IEnumerable<Project>> GetProjectsByLocationAsync(int locationId);
        Task<IEnumerable<Project>> GetAllProjectWithLocationAsync();

    }
}
