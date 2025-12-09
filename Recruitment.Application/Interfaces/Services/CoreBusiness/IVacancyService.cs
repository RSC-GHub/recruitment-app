using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IVacancyService
    {
        Task<List<VacancyListDTO>> GetAllVacanciesAsync();
        Task<List<VacancyListDTO>> GetOpenedVacanciesAsync();
        Task<VacancyDetailsDTO?> GetVacancyByIdAsync(int id);
        Task<VacancyDetailsDTO> CreateVacancyAsync(VacancyCreateDTO dto);
        Task<VacancyDetailsDTO?> UpdateVacancyAsync(VacancyUpdateDTO dto);
        Task<bool> DeleteVacancyAsync(int id);
        Task<bool> TitleNameExistsAsync(string name, int? excludeId = null);

        Task<PagedResult<VacancyListDTO>> SearchAsync(
                string? search, int? titleId, int? projectId, VacancyStatus? status,
                int page, int pageSize);
        Task<List<VacancyListDTO>> FilterVacanciesAsync(int? titleId, int? projectId, VacancyStatus? status);

        Task<int> CountOpenedVacanciesAsync();
    }
}
