using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<Project?> GetProjectWithVacanciesAsync(int projectId);
        Task<Project?> GetProjectWithLocationAsync(int projectId);

        Task<IEnumerable<Project>> GetProjectsByLocationAsync(int locationId);
        Task<IEnumerable<Project>> GetAllProjectWithLocationAsync();

    }
}
