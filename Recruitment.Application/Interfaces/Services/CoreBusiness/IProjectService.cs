using Recruitment.Application.DTOs.CoreBusiness.Project;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int projectId);
        Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto);
        Task<ProjectDto?> UpdateProjectAsync(ProjectUpdateDto projectDto);
        Task<bool> DeleteProjectAsync(int projectId);
        Task<ProjectDto?> GetProjectWithLocationAsync(int projectId);
        Task<IEnumerable<ProjectDto>> GetAllProjectWithLocationAsync();
        Task<ProjectDto?> GetProjectWithVacanciesAsync(int projectId);
        Task<IEnumerable<ProjectDto>> GetProjectsByLocationAsync(int locationId);
    }
}
