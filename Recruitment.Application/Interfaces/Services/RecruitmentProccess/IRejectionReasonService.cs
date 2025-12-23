using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.RejectionReason;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.RecruitmentProccess
{
    public interface IRejectionReasonService
    {
        Task<PagedResult<ReasonDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null);
        Task<ReasonDto?> GetByIdAsync(int id);
        Task AddAsync(CreateReasonDto dto);
        Task UpdateAsync(ReasonDto dto);
        Task DeleteAsync(int id);

        Task<List<ReasonDto>> GetAllAsync();
        Task<List<ReasonDto>> GetByTypeAsync(RejectionReasonType reasonType);

    }
}
