using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Project;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Mapping helper
        private ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Status = project.Status, 
                LocationId = project.LocationId,
                LocationName = project.Location?.Name
            };
        }

        public async Task<PagedResult<ProjectDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? countryId = null)
        {
            var pagedProjects = await _unitOfWork.ProjectRepository
                                                 .GetPagedAsync(page, pageSize, search, countryId);

            var dtoItems = pagedProjects.Items.Select(p => new ProjectDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Status = p.Status,
                LocationId = p.LocationId,
                LocationName = p.Location != null
                        ? $"{p.Location.Name} - {p.Location.Country!.Name}"
                        : null

            }).ToList();

            return new PagedResult<ProjectDto>(
                dtoItems,
                pagedProjects.TotalCount,
                page,
                pageSize
            );
        }


        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            return projects.Select(MapToDto);
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int projectId)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            if (project == null) return null;
            return MapToDto(project);
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto dto)
        {
            var project = new Project
            {
                ProjectName = dto.ProjectName,
                Status = dto.Status, 
                LocationId = dto.LocationId
            };

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync();

            return MapToDto(project);
        }

        public async Task<ProjectDto?> UpdateProjectAsync(ProjectUpdateDto dto)
        {
            var existing = await _unitOfWork.Projects.GetByIdAsync(dto.Id);
            if (existing == null) return null;

            existing.ProjectName = dto.ProjectName;
            existing.Status = dto.Status; 
            existing.LocationId = dto.LocationId;

            _unitOfWork.Projects.Update(existing);
            await _unitOfWork.CompleteAsync();

            return MapToDto(existing);
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var existing = await _unitOfWork.Projects.GetByIdAsync(projectId);
            if (existing == null) return false;

            _unitOfWork.Projects.Delete(existing);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectWithLocationAsync()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllProjectWithLocationAsync();

            var dtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Status = p.Status,
                LocationId = p.LocationId,
                LocationName = p.Location?.Name
            });

            return dtos;
        }

        public async Task<ProjectDto?> GetProjectWithLocationAsync(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectWithLocationAsync(projectId);
            if (project == null) return null;

            var dto = new ProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Status = project.Status,
                LocationId = project.LocationId,
                LocationName = project.Location?.Name
            };

            return dto;
        }

        public async Task<ProjectDto?> GetProjectWithVacanciesAsync(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectWithVacanciesAsync(projectId);
            if (project == null) return null;

            var dto = MapToDto(project);
            
            return dto;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByLocationAsync(int locationId)
        {
            var projects = await _unitOfWork.ProjectRepository.GetProjectsByLocationAsync(locationId);
            return projects.Select(MapToDto);
        }
    }
}
