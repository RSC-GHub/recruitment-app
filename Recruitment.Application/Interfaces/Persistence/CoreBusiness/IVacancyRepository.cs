using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;


namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface IVacancyRepository : IGenericRepository<Vacancy>
    {
        Task<Vacancy?> GetVacancyByIdAsync(int id);
        Task<List<Vacancy>> GetAllVacanciesWithProjectsAsync();

        Task<List<Vacancy>> GetAllOpenedVacancies();

        Task<bool> NameExistsAsync(string name, int? excludeId = null);

        Task<PagedResult<Vacancy>> SearchAsync(
            string? search,
            int? titleId,
            int? projectId,
            VacancyStatus? status,
            int page,
            int pageSize);

        Task<List<Vacancy>> FilterAsync(int? titleId, int? projectId, VacancyStatus? status);

        Task<PagedResult<Vacancy>> GetPagedAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Vacancy?> GetForEditAsync(int id);
        Task<List<Vacancy>> GetAllOpenedVacanciesCards();

        Task<List<VacancyPositionsChartDTO>> GetVacanciesPositionsChartAsync();
        void RemoveProjectVacancies(IEnumerable<ProjectVacancy> pvs);
    }
}
