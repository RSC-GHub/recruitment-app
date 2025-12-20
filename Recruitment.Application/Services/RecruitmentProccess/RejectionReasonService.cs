using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.RejectionReason;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;

namespace Recruitment.Application.Services.RecruitmentProccess
{
    public class RejectionReasonService : IRejectionReasonService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RejectionReasonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(CreateReasonDto dto)
        {
            var reason = new Domain.Entities.RecruitmentProccess.RejectionReason
            {
                Reason = dto.Reason,
            };
            await _unitOfWork.RejectionReasons.AddAsync(reason);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reason = await _unitOfWork.RejectionReasons.GetByIdAsync(id);
            if (reason == null)
                throw new Exception("Rejection Reason not found");
            _unitOfWork.RejectionReasons.Delete(reason);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<PagedResult<ReasonDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            var pagedResult =
                await _unitOfWork.RejectionReasonRepository
                                 .GetPagedAsync(page, pageSize, search);

            var dtoItems = pagedResult.Items.Select(r => new ReasonDto
            {
                Id = r.Id,
                Reason = r.Reason
            }).ToList();

            return new PagedResult<ReasonDto>(
                dtoItems,
                pagedResult.TotalCount,
                page,
                pageSize
            );
        }


        public async Task<ReasonDto?> GetByIdAsync(int id)
        {
            var reason = await _unitOfWork.RejectionReasons.GetByIdAsync(id);
            if (reason == null)
            {
                return null;
            }
            return new ReasonDto
            {
                Id = reason.Id,
                Reason = reason.Reason,
            };
        }

        public async Task UpdateAsync(ReasonDto dto)
        {
            var reason = await _unitOfWork.RejectionReasons.GetByIdAsync(dto.Id);
            if (reason == null)
                throw new Exception("Rejection Reason not found");
            reason.Reason = dto.Reason;
            _unitOfWork.RejectionReasons.Update(reason);
            await _unitOfWork.CompleteAsync();
        }
    }
}
