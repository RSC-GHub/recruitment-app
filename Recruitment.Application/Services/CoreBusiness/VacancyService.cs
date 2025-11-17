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

        // --------------------------------------------
        //  GET TABLE VIEW
        // --------------------------------------------
        public async Task<List<VacancyTableDto>> GetVacanciesForTableAsync()
        {
            var vacancies = await _vacancyRepository.GetAllVacanciesWithProjectsAsync();

            return vacancies.Select(v => new VacancyTableDto
            {
                Id = v.Id,
                TitleName = v.Title?.Name ?? "",
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType,
                Status = v.Status,
                Projects = v.ProjectVacancies?.Select(pv => new ProjectVacancyTableDto
                {
                    ProjectName = pv.Project?.ProjectName ?? "",
                    Priority = pv.Priority
                }).ToList() ?? new List<ProjectVacancyTableDto>()
            }).ToList();
        }

        // --------------------------------------------
        //  GET DETAILS VIEW
        // --------------------------------------------
        public async Task<VacancyViewDto?> GetVacancyDetailsAsync(int id)
        {
            var vacancy = await _vacancyRepository.GetVacancyByIdAsync(id);
            if (vacancy == null) return null;

            return new VacancyViewDto
            {
                Id = vacancy.Id,
                TitleId = vacancy.TitleId,
                TitleName = vacancy.Title?.Name ?? "",
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
                Projects = vacancy.ProjectVacancies?.Select(pv => new ProjectVacancyViewDto
                {
                    ProjectId = pv.ProjectId,
                    ProjectName = pv.Project?.ProjectName ?? "",
                    Priority = pv.Priority
                }).ToList() ?? new List<ProjectVacancyViewDto>()
            };
        }

        // --------------------------------------------
        //  CREATE
        // --------------------------------------------
        public async Task<int> CreateVacancyAsync(VacancyCreateDto dto)
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
                Deadline = dto.Deadline,
                Status = Domain.Enums.VacancyStatus.Open
            };

            await _unitOfWork.Vacancies.AddAsync(vacancy);
            await _unitOfWork.CompleteAsync();

            // Add project relations
            foreach (var p in dto.Projects)
            {
                var pv = new ProjectVacancy
                {
                    ProjectId = p.ProjectId,
                    VacancyId = vacancy.Id,
                    Priority = p.Priority
                };

                await _unitOfWork.ProjectVacancies.AddAsync(pv);
            }

            await _unitOfWork.CompleteAsync();
            return vacancy.Id;
        }

        // --------------------------------------------
        //  UPDATE
        // --------------------------------------------
        public async Task<bool> UpdateVacancyAsync(int id, VacancyUpdateDto dto)
        {
            var vacancy = await _vacancyRepository.GetVacancyWithProjectsAsync(id);
            if (vacancy == null) return false;

            // update main fields
            vacancy.TitleId = dto.TitleId;
            vacancy.JobDescription = dto.JobDescription;
            vacancy.Requirements = dto.Requirements;
            vacancy.Responsibilities = dto.Responsibilities;
            vacancy.Benefits = dto.Benefits;
            vacancy.PositionCount = dto.PositionCount;
            vacancy.EmploymentType = dto.EmploymentType;
            vacancy.SalaryRangeMin = dto.SalaryRangeMin;
            vacancy.SalaryRangeMax = dto.SalaryRangeMax;
            vacancy.Deadline = dto.Deadline;

            // update ProjectVacancies
            var existingRelations = vacancy.ProjectVacancies?.ToList() ?? new List<ProjectVacancy>();

            // delete removed projects
            foreach (var old in existingRelations)
            {
                if (!dto.Projects.Any(p => p.ProjectId == old.ProjectId))
                    _unitOfWork.ProjectVacancies.Delete(old);
            }

            // add or update projects
            foreach (var newP in dto.Projects)
            {
                var existing = existingRelations.FirstOrDefault(x => x.ProjectId == newP.ProjectId);

                if (existing == null)
                {
                    var pv = new ProjectVacancy
                    {
                        ProjectId = newP.ProjectId,
                        VacancyId = vacancy.Id,
                        Priority = newP.Priority
                    };
                    await _unitOfWork.ProjectVacancies.AddAsync(pv);
                }
                else
                {
                    existing.Priority = newP.Priority;
                    _unitOfWork.ProjectVacancies.Update(existing);
                }
            }

            _unitOfWork.Vacancies.Update(vacancy);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // --------------------------------------------
        //  DELETE
        // --------------------------------------------
        public async Task<bool> DeleteVacancyAsync(int id)
        {
            var vacancy = await _unitOfWork.Vacancies.GetByIdAsync(id);
            if (vacancy == null) return false;

            _unitOfWork.Vacancies.Delete(vacancy);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
