using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Department;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class DepartmentService :  IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<DepartmentDto>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null)
        {
            var pagedResult =
                await _unitOfWork.DepartmentRepository.GetPagedAsync(page, pageSize, search);

            var dtoItems = pagedResult.Items.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            return new PagedResult<DepartmentDto>(
                dtoItems,
                pagedResult.TotalCount,
                page,
                pageSize
            );
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            });
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
                return null;

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name
            };
        }

        public async Task AddAsync(CreateDepartmentDto dto)
        {
            var entity = new Department
            {
                Name = dto.Name
            };

            await _unitOfWork.Departments.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateDepartmentDto dto)
        {
            var existing = await _unitOfWork.Departments.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Department with Id {dto.Id} not found.");

            existing.Name = dto.Name;
            _unitOfWork.Departments.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");

            _unitOfWork.Departments.Delete(department);
            await _unitOfWork.CompleteAsync();
        }
    }
}
