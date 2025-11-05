using Recruitment.Application.DTOs.CoreBusiness.DepartmentTitle;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IDepartmentTitleService
    {
        Task<IEnumerable<DepartmentTitleDto>> GetAllAsync();
        Task<DepartmentTitleDto?> GetByIdAsync(int id);
        Task AddAsync(CreateDepartmentTitleDto dto);
        Task UpdateAsync(UpdateDepartmentTitleDto dto);
        Task DeleteAsync(int id);
    }
}
