using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

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

        private VacancyDto MapToDto(Vacancy vacancy)
        {
            return new VacancyDto
            {
                Id = vacancy.Id,
                TitleId = vacancy.TitleId,
                TitleName = vacancy.Title?.Name,
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType,
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Status = vacancy.Status,
                Deadline = vacancy.Deadline,
                Projects = vacancy.ProjectVacancies?.Select(pv => new ProjectPriorityDto
                {
                    ProjectId = pv.ProjectId,
                    ProjectName = pv.Project?.ProjectName, 
                    Priority = pv.Priority
                }).ToList() ?? new List<ProjectPriorityDto>()

            };
        }


        public async Task<IEnumerable<VacancyDto>> GetAllVacanciesAsync()
        {
            var vacancies = await _unitOfWork.Vacancies.GetAllAsync();
            return vacancies.Select(MapToDto);
        }

        public async Task<VacancyDto?> GetVacancyByIdAsync(int id)
        {
            var vacancy = await _unitOfWork.Vacancies.GetByIdAsync(id);
            if (vacancy == null) return null;
            return MapToDto(vacancy);
        }

        public async Task<VacancyDto> CreateVacancyAsync(VacancyCreateDto dto)
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

            if (dto.Projects != null && dto.Projects.Any())
            {
                vacancy.ProjectVacancies = dto.Projects
                    .Select(p => new ProjectVacancy { ProjectId = p.ProjectId, Priority = p.Priority })
                    .ToList();
            }

            await _unitOfWork.Vacancies.AddAsync(vacancy);
            await _unitOfWork.CompleteAsync();

            return MapToDto(vacancy);
        }


        public async Task<VacancyDto?> UpdateVacancyAsync(VacancyUpdateDto dto)
        {
            var vacancy = await _vacancyRepository.GetVacancyWithProjectsAsync(dto.Id);
            if (vacancy == null) return null;

            vacancy.TitleId = dto.TitleId;
            vacancy.JobDescription = dto.JobDescription;
            vacancy.Requirements = dto.Requirements;
            vacancy.Responsibilities = dto.Responsibilities;
            vacancy.Benefits = dto.Benefits;
            vacancy.PositionCount = dto.PositionCount;
            vacancy.EmploymentType = dto.EmploymentType;
            vacancy.SalaryRangeMin = dto.SalaryRangeMin;
            vacancy.SalaryRangeMax = dto.SalaryRangeMax;
            vacancy.Status = dto.Status;
            vacancy.Deadline = dto.Deadline;

            vacancy.ProjectVacancies ??= new List<ProjectVacancy>();

            foreach (var projDto in dto.Projects)
            {
                var existing = vacancy.ProjectVacancies.FirstOrDefault(pv => pv.ProjectId == projDto.ProjectId);
                if (existing != null)
                {
                    existing.Priority = projDto.Priority;
                }
                else
                {
                    vacancy.ProjectVacancies.Add(new ProjectVacancy
                    {
                        ProjectId = projDto.ProjectId,
                        Priority = projDto.Priority,
                        VacancyId = vacancy.Id
                    });
                }
            }

            var dtoProjectIds = dto.Projects.Select(p => p.ProjectId).ToList();
            var toRemove = vacancy.ProjectVacancies
                .Where(pv => !dtoProjectIds.Contains(pv.ProjectId))
                .ToList();
            foreach (var r in toRemove)
            {
                _unitOfWork.ProjectVacancies.Delete(r); 
            }

            _unitOfWork.Vacancies.Update(vacancy);
            await _unitOfWork.CompleteAsync();

            return MapToDto(vacancy);
        }


        public async Task<bool> DeleteVacancyAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetVacancyWithProjectsAsync(id);
            if (vacancy == null) return false;

            if (vacancy.ProjectVacancies != null && vacancy.ProjectVacancies.Any())
            {
                foreach (var pv in vacancy.ProjectVacancies)
                {
                    _unitOfWork.ProjectVacancies.Delete(pv); 
                }
            }

            _unitOfWork.Vacancies.Delete(vacancy);

            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<IEnumerable<VacancyDto>> GetOpenVacanciesAsync()
        {
            var vacancies = await _vacancyRepository.GetOpenVacanciesAsync();
            return vacancies.Select(MapToDto);
        }

        public async Task<VacancyDto?> GetVacancyWithProjectsAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetVacancyWithProjectsAsync(id);
            if (vacancy == null) return null;
            return MapToDto(vacancy);
        }

        public async Task<IEnumerable<VacancyDto>> GetAllVacanciesWithProjectsAsync()
        {
            var vacancies = await _vacancyRepository.GetAllVacanciesWithProjectsAsync();
            return vacancies.Select(MapToDto);
        }
    }
}
