using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface IVacancyRepository : IGenericRepository<Vacancy>
    {
        Task<Vacancy?> GetVacancyWithProjectsAsync(int id);
        Task<IEnumerable<Vacancy>> GetAllVacanciesWithProjectsAsync();
        Task<IEnumerable<Vacancy>> GetOpenVacanciesAsync();

    }
}
