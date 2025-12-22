using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Services.Common;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class VacancyService : IVacancyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VacancyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<VacancyListDTO>> GetOpenedVacanciesAsync()
        {
            var vacancies = await _unitOfWork.VacancyRepository.GetAllOpenedVacancies();
            return vacancies.Select(v => new VacancyListDTO
            {
                Id = v.Id,
                TitleName = v.Title?.Name ?? ""
            }).ToList();
        }
        public async Task<List<VacancyListDTO>> GetAllVacanciesAsync()
        {
            var vacancies = await _unitOfWork.VacancyRepository.GetAllVacanciesWithProjectsAsync();

            return vacancies.Select(v => new VacancyListDTO
            {
                Id = v.Id,
                TitleName = v.Title?.Name ?? "",
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType.ToString(),
                Status = v.Status.ToString(),
                Deadline = v.Deadline,
                ProjectNames = v.ProjectVacancies?.Select(pv => pv.Project?.ProjectName ?? "").ToList() ?? new List<string>()
            }).ToList();
        }

        public async Task<VacancyDetailsDTO?> GetVacancyByIdAsync(int id)
        {
            var vacancy = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);
            if (vacancy == null) return null;

            return new VacancyDetailsDTO
            {
                Id = vacancy.Id,
                TitleName = vacancy.Title?.Name ?? "",
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType.ToString(),
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Status = vacancy.Status.ToString(),
                Deadline = vacancy.Deadline,
                ProjectNames = vacancy.ProjectVacancies?
                                    .Select(pv => pv.Project?.ProjectName ?? "")
                                    .ToList() ?? new List<string>(),

                // Audit
                CreatedBy = vacancy.CreatedBy,
                CreatedOn = vacancy.CreatedOn,
                ModifiedBy = vacancy.ModifiedBy,
                ModifiedOn = vacancy.ModifiedOn
            };
        }


        public async Task<VacancyDetailsDTO> CreateVacancyAsync(VacancyCreateDTO dto)
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
                Status = VacancyStatus.Open
            };

            if (dto.ProjectIds != null && dto.ProjectIds.Any())
            {
                vacancy.ProjectVacancies = dto.ProjectIds
                    .Distinct()
                    .Select(pid => new ProjectVacancy { ProjectId = pid })
                    .ToList();
            }

            await _unitOfWork.VacancyRepository.AddAsync(vacancy);
            await _unitOfWork.CompleteAsync();

            return await GetVacancyByIdAsync(vacancy.Id)
                   ?? throw new Exception("Error creating vacancy");
        }

        public async Task<VacancyDetailsDTO?> UpdateVacancyAsync(VacancyUpdateDTO dto)
        {
            var vacancy = await _unitOfWork.VacancyRepository.GetForEditAsync(dto.Id);
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

            var existingProjectIds = vacancy.ProjectVacancies?.Select(pv => pv.ProjectId).ToList() ?? new List<int>();
            var newProjectIds = dto.ProjectIds?.Distinct().ToList() ?? new List<int>();

            var toRemove = vacancy.ProjectVacancies?.Where(pv => !newProjectIds.Contains(pv.ProjectId)).ToList();
            if (toRemove != null)
                foreach (var pv in toRemove)
                    vacancy.ProjectVacancies.Remove(pv);

            var toAdd = newProjectIds.Except(existingProjectIds);
            foreach (var pid in toAdd)
                vacancy.ProjectVacancies.Add(new ProjectVacancy { ProjectId = pid, VacancyId = vacancy.Id });

            _unitOfWork.VacancyRepository.Update(vacancy);
            await _unitOfWork.CompleteAsync();

            return await GetVacancyByIdAsync(vacancy.Id);
        }

        public async Task<bool> DeleteVacancyAsync(int id)
        {
            var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(id);
            if (vacancy == null) return false;

            _unitOfWork.VacancyRepository.Delete(vacancy);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> TitleNameExistsAsync(string name, int? excludeId = null)
        {
            return await _unitOfWork.VacancyRepository.NameExistsAsync(name, excludeId);
        }

        public async Task<PagedResult<VacancyListDTO>> SearchAsync(
            string? search, int? titleId, int? projectId, VacancyStatus? status,
            int page, int pageSize)
        {
            var paged = await _unitOfWork.VacancyRepository.SearchAsync(
                search, titleId, projectId, status, page, pageSize);

            var dtoItems = paged.Items.Select(v => new VacancyListDTO
            {
                Id = v.Id,
                TitleName = v.Title.Name,
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType.ToString(),
                Status = v.Status.ToString(),
                Deadline = v.Deadline,
                ProjectNames = v.ProjectVacancies.Select(pv => pv.Project.ProjectName).ToList()
            }).ToList();

            return new PagedResult<VacancyListDTO>(dtoItems, paged.TotalCount, page, pageSize);
        }


        public async Task<List<VacancyListDTO>> FilterVacanciesAsync(int? titleId, int? projectId, VacancyStatus? status)
        {
            var vacancies = await _unitOfWork.VacancyRepository.FilterAsync(titleId, projectId, status);
            return vacancies.Select(v => new VacancyListDTO
            {
                Id = v.Id,
                TitleName = v.Title?.Name ?? "",
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType.ToString(),
                Status = v.Status.ToString(),
                Deadline = v.Deadline,
                ProjectNames = v.ProjectVacancies?.Select(pv => pv.Project?.ProjectName ?? "").ToList() ?? new List<string>()
            }).ToList();
        }

        public async Task<int> CountOpenedVacanciesAsync()
        {
            var vacancies = await _unitOfWork.VacancyRepository.GetAllOpenedVacancies();
            return vacancies.Count;
        }

        public async Task<VacancyDetailsDTO?> GetVacancyByIdAsyncForAPI(int id)
        {
            var vacancy = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);
            if (vacancy == null) return null;
            return new VacancyDetailsDTO
            {
                Id = vacancy.Id,
                TitleName = TextHelper.CleanText(vacancy.Title?.Name ?? ""),
                JobDescription = TextHelper.CleanText(vacancy.JobDescription),
                Requirements = TextHelper.CleanText(vacancy.Requirements),
                Responsibilities = TextHelper.CleanText(vacancy.Responsibilities),
                Benefits = TextHelper.CleanText(vacancy.Benefits),
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType.ToString(),
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
            };
        }

        public async Task<List<VacancyCardDTO>> GetVacancyCardsAsync(int shortTextLength = 200)
        {
            var vacancies = await _unitOfWork.VacancyRepository.GetAllOpenedVacanciesCards();

            if (vacancies == null || !vacancies.Any())
                return new List<VacancyCardDTO>();

            return vacancies.Select(v => new VacancyCardDTO
            {
                Id = v.Id,
                TitleName = TextHelper.CleanText(v.Title?.Name ?? ""),
                Department = v.Title?.DepartmentTitles?.FirstOrDefault()?.Department?.Name ?? "",
                EmploymentType = v.EmploymentType.ToString(),
                Location = v.ProjectVacancies?.Select(pv => pv.Project?.Location?.Name ?? "")
                                 .FirstOrDefault() ?? "",
                ShortDescription = TextHelper.TruncateText(TextHelper.CleanText(v.JobDescription ?? ""), shortTextLength),
                KeyRequirements = TextHelper.TruncateText(TextHelper.CleanText(v.Requirements ?? ""), shortTextLength)
            }).ToList();
        }
    }
}
