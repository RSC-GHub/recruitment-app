using Recruitment.Application.DTOs.CoreBusiness.Department;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task AddAsync(CreateDepartmentDto dto);
        Task UpdateAsync(UpdateDepartmentDto dto);
        Task DeleteAsync(int id);
    }
}
