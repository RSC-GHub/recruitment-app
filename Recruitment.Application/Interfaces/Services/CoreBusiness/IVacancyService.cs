using Recruitment.Application.DTOs.CoreBusiness.Vacancy;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IVacancyService
    {
        Task<List<VacancyTableDto>> GetVacanciesForTableAsync();

        Task<VacancyViewDto?> GetVacancyDetailsAsync(int id);

        Task<int> CreateVacancyAsync(VacancyCreateDto dto);

        Task<bool> UpdateVacancyAsync(int id, VacancyUpdateDto dto);

        Task<bool> DeleteVacancyAsync(int id);
    }
}
