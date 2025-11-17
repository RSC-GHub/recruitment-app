using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface IVacancyRepository : IGenericRepository<Vacancy>
    {
        Task<Vacancy?> GetVacancyWithProjectsAsync(int id);
        Task<List<Vacancy>> GetAllVacanciesWithProjectsAsync();
        Task<Vacancy?> GetVacancyByIdAsync(int id);
    }
}
