using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IVacancyService
    {
        Task<VacancyDetailsDto?> GetByIdAsync(int id);
        Task<List<VacancyListDto>> GetAllAsync();
        Task<PagedResult<VacancyListDto>> GetPagedAsync(int page, int pageSize);
        Task<List<VacancyListDto>> SearchAsync(string? keyword);
        Task<List<VacancyListDto>> FilterAsync(int? titleId, int? projectId, VacancyStatus? status);
        Task<VacancyDetailsDto> CreateAsync(VacancyCreateDto dto);
        Task UpdateAsync(VacancyUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
