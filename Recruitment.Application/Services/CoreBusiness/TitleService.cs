using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.Interfaces.Persistence;
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

        public async Task AddAsync(CreateTitleDto dto)
        {
            var title = new Title
            {
                Name = dto.Name
            };

            await _unitOfWork.Titles.AddAsync(title);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateTitleDto dto)
        {
            var existing = await _unitOfWork.Titles.GetByIdAsync(dto.Id);
            if (existing == null)
                return;

            existing.Name = dto.Name;
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
