using Recruitment.Application.DTOs.CoreBusiness.Department;
using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class TitleService : ITitleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TitleDto>> GetAllAsync()
        {
            var titles = await _unitOfWork.Titles.GetAllAsync();
            return titles.Select(t => new TitleDto
            {
                Id = t.Id,
                Name = t.Name
            });
        }

        public async Task<TitleDto?> GetByIdAsync(int id)
        {
            var title = await _unitOfWork.Titles.GetByIdAsync(id);
            if (title == null)
                return null;

            return new TitleDto
            {
                Id = title.Id,
                Name = title.Name
            };
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByTitleIdAsync(int titleId)
        {
            var title = await _unitOfWork.Titles.GetByIdAsync(titleId);
            if (title == null) return Enumerable.Empty<DepartmentDto>();

            var departmentTitles = await _unitOfWork.DepartmentTitles
                .FindAsync(dt => dt.TitleId == titleId);

            var departmentIds = departmentTitles.Select(dt => dt.DepartmentId).ToList();
            var departments = await _unitOfWork.Departments
                .FindAsync(d => departmentIds.Contains(d.Id));

            return departments.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name });
        }

        public async Task AddAsync(CreateTitleDto dto)
        {
            var title = new Title { Name = dto.Name };
            await _unitOfWork.Titles.AddAsync(title);
            await _unitOfWork.CompleteAsync();

            if (dto.DepartmentIds.Any())
            {
                foreach (var depId in dto.DepartmentIds)
                {
                    var departmentTitle = new DepartmentTitle
                    {
                        DepartmentId = depId,
                        TitleId = title.Id
                    };
                    await _unitOfWork.DepartmentTitles.AddAsync(departmentTitle);
                }
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<TitleDto?> GetByIdWithDepartmentsAsync(int id)
        {
            var entity = await _unitOfWork.TitleRepository.GetByIdWithDepartmentsAsync(id);
            if (entity == null) return null;

            return new TitleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DepartmentIds = entity.DepartmentTitles?
                                      .Select(dt => dt.DepartmentId)
                                      .ToList() ?? new List<int>()
            };
        }

        public async Task UpdateAsync(UpdateTitleDto dto)
        {
            var existing = await _unitOfWork.Titles.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Title with Id {dto.Id} not found.");

            // update basic properties
            existing.Name = dto.Name;

            // update DepartmentTitle relations
            var currentDeptTitles = await _unitOfWork.DepartmentTitles
                .FindAsync(dt => dt.TitleId == dto.Id);

            // remove unselected
            foreach (var dt in currentDeptTitles)
            {
                if (!dto.DepartmentIds.Contains(dt.DepartmentId))
                    _unitOfWork.DepartmentTitles.Delete(dt);
            }

            // add newly selected
            var currentDeptIds = currentDeptTitles.Select(dt => dt.DepartmentId).ToList();
            foreach (var deptId in dto.DepartmentIds)
            {
                if (!currentDeptIds.Contains(deptId))
                {
                    var newDeptTitle = new DepartmentTitle
                    {
                        TitleId = dto.Id,
                        DepartmentId = deptId
                    };
                    await _unitOfWork.DepartmentTitles.AddAsync(newDeptTitle);
                }
            }

            _unitOfWork.Titles.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var title = await _unitOfWork.Titles.GetByIdAsync(id);
            if (title == null)
                return;

            _unitOfWork.Titles.Delete(title);
            await _unitOfWork.CompleteAsync();
        }
    }
}
