using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class VacancyService : IVacancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyService(IUnitOfWork unitOfWork, IVacancyRepository vacancyRepository)
        {
            _unitOfWork = unitOfWork;
            _vacancyRepository = vacancyRepository;
        }

        private VacancyDetailsDto MapToDetailsDto(Vacancy v)
        {
            return new VacancyDetailsDto
            {
                Id = v.Id,
                TitleName = v.Title?.Name ?? "",
                JobDescription = v.JobDescription,
                Requirements = v.Requirements,
                Responsibilities = v.Responsibilities,
                Benefits = v.Benefits,
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType,
                SalaryRangeMin = v.SalaryRangeMin,
                SalaryRangeMax = v.SalaryRangeMax,
                Status = v.Status,
                Deadline = v.Deadline,
                Projects = v.ProjectVacancies?
                    .Select(p => new ProjectSummaryDto
                    {
                        ProjectId = p.ProjectId,
                        ProjectName = p.Project?.ProjectName ?? ""
                    })
                    .ToList() ?? new List<ProjectSummaryDto>()
            };
        }

        private VacancyListDto MapToListDto(Vacancy v)
        {
            return new VacancyListDto
            {
                Id = v.Id,
                TitleName = v.Title?.Name ?? "",
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType,
                Status = v.Status,
                ProjectNames = v.ProjectVacancies?
                    .Select(p => p.Project?.ProjectName ?? "")
                    .ToList() ?? new List<string>()
            };
        }

        private void MapUpdateToEntity(VacancyUpdateDto dto, Vacancy v)
        {
            v.TitleId = dto.TitleId;
            v.JobDescription = dto.JobDescription;
            v.Requirements = dto.Requirements;
            v.Responsibilities = dto.Responsibilities;
            v.Benefits = dto.Benefits;
            v.PositionCount = dto.PositionCount;
            v.EmploymentType = dto.EmploymentType;
            v.SalaryRangeMin = dto.SalaryRangeMin;
            v.SalaryRangeMax = dto.SalaryRangeMax;
            v.Status = dto.Status;
            v.Deadline = dto.Deadline;
        }

        public async Task<VacancyDetailsDto?> GetByIdAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetVacancyByIdAsync(id);

            return vacancy == null ? null : MapToDetailsDto(vacancy);
        }

        public async Task<List<VacancyListDto>> GetAllAsync()
        {
            var vacancies = await _vacancyRepository.GetAllVacanciesWithProjectsAsync();
            return vacancies.Select(MapToListDto).ToList();
        }

        public async Task<PagedResult<VacancyListDto>> GetPagedAsync(int page, int pageSize)
        {
            var paged = await _vacancyRepository.GetPagedAsync(page, pageSize);

            return new PagedResult<VacancyListDto>
            {
                Items = paged.Items.Select(MapToListDto).ToList(),
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            };
        }

        public async Task<List<VacancyListDto>> SearchAsync(string? keyword)
        {
            var data = await _vacancyRepository.SearchAsync(keyword);
            return data.Select(MapToListDto).ToList();
        }

        public async Task<List<VacancyListDto>> FilterAsync(int? titleId, int? projectId, VacancyStatus? status)
        {
            var data = await _vacancyRepository.FilterAsync(titleId, projectId, status);
            return data.Select(MapToListDto).ToList();
        }

        public async Task<VacancyDetailsDto> CreateAsync(VacancyCreateDto dto)
        {
            var vacancy = new Vacancy
            {
                TitleId = dto.TitleId,
                JobDescription = dto.JobDescription,
                Requirements = dto.Requirements,
                Responsibilities = dto.Responsibilities,
                Benefits = dto.Benefits,
                PositionCount = dto.PositionCount,
                EmploymentType = dto.EmploymentType,
                SalaryRangeMin = dto.SalaryRangeMin,
                SalaryRangeMax = dto.SalaryRangeMax,
                Status = dto.Status,
                Deadline = dto.Deadline
            };

            await _unitOfWork.Vacancies.AddAsync(vacancy);
            await _unitOfWork.CompleteAsync();

            return MapToDetailsDto(vacancy);
        }

        public async Task UpdateAsync(VacancyUpdateDto dto)
        {
            var vacancy = await _vacancyRepository.GetForEditAsync(dto.Id);

            if (vacancy == null)
                throw new KeyNotFoundException($"Vacancy with Id {dto.Id} not found.");

            MapUpdateToEntity(dto, vacancy);

            _unitOfWork.Vacancies.Update(vacancy);

            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vacancy = await _unitOfWork.Vacancies.GetByIdAsync(id);

            if (vacancy == null)
                return;

            _unitOfWork.Vacancies.Delete(vacancy);
            await _unitOfWork.CompleteAsync();
        }
    }
}
