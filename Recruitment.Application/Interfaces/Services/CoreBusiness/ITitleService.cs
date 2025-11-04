using Recruitment.Application.DTOs.CoreBusiness.Title;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface ITitleService
    {
        Task<IEnumerable<TitleDto>> GetAllAsync();
        Task<TitleDto?> GetByIdAsync(int id);
        Task AddAsync(CreateTitleDto dto);
        Task UpdateAsync(UpdateTitleDto dto);
        Task DeleteAsync(int id);
    }
}
