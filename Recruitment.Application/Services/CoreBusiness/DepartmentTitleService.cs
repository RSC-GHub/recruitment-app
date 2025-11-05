using Recruitment.Application.DTOs.CoreBusiness.DepartmentTitle;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class DepartmentTitleService : IDepartmentTitleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentTitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentTitleDto>> GetAllAsync()
        {
            var list = await _unitOfWork.DepartmentTitles.GetAllAsync();
            var result = new List<DepartmentTitleDto>();

            foreach (var entity in list)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(entity.DepartmentId);
                var title = await _unitOfWork.Titles.GetByIdAsync(entity.TitleId);

                result.Add(new DepartmentTitleDto
                {
                    Id = entity.Id,
                    DepartmentId = entity.DepartmentId,
                    DepartmentName = department?.Name,
                    TitleId = entity.TitleId,
                    TitleName = title?.Name
                });
            }

            return result;
        }

        public async Task<DepartmentTitleDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.DepartmentTitles.GetByIdAsync(id);
            if (entity == null) return null;

            var department = await _unitOfWork.Departments.GetByIdAsync(entity.DepartmentId);
            var title = await _unitOfWork.Titles.GetByIdAsync(entity.TitleId);

            return new DepartmentTitleDto
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                DepartmentName = department?.Name,
                TitleId = entity.TitleId,
                TitleName = title?.Name
            };
        }

        public async Task AddAsync(CreateDepartmentTitleDto dto)
        {
            var entity = new DepartmentTitle
            {
                DepartmentId = dto.DepartmentId,
                TitleId = dto.TitleId
            };

            await _unitOfWork.DepartmentTitles.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateDepartmentTitleDto dto)
        {
            var existing = await _unitOfWork.DepartmentTitles.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"DepartmentTitle with Id {dto.Id} not found.");

            existing.DepartmentId = dto.DepartmentId;
            existing.TitleId = dto.TitleId;

            _unitOfWork.DepartmentTitles.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.DepartmentTitles.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"DepartmentTitle with Id {id} not found.");

            _unitOfWork.DepartmentTitles.Delete(entity);
            await _unitOfWork.CompleteAsync();
        }
    }
}
