using Recruitment.Application.Common;
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

        Task<List<Vacancy>> SearchAsync(string? keyword);

        Task<List<Vacancy>> FilterAsync(int? titleId, int? projectId, VacancyStatus? status);

        Task<PagedResult<Vacancy>> GetPagedAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Vacancy?> GetForEditAsync(int id);
    }
}
