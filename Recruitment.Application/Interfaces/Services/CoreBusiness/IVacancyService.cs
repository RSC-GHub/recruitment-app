using Recruitment.Application.DTOs.CoreBusiness.Vacancy;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IVacancyService
    {
        Task<IEnumerable<VacancyDto>> GetAllVacanciesAsync();

        Task<VacancyDto?> GetVacancyByIdAsync(int id);

        Task<VacancyDto> CreateVacancyAsync(VacancyCreateDto dto);

        Task<VacancyDto?> UpdateVacancyAsync(VacancyUpdateDto dto);

        Task<bool> DeleteVacancyAsync(int id);

        Task<IEnumerable<VacancyDto>> GetOpenVacanciesAsync();

        Task<VacancyDto?> GetVacancyWithProjectsAsync(int id);

        Task<IEnumerable<VacancyDto>> GetAllVacanciesWithProjectsAsync();
    }
}
