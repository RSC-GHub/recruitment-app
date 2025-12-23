using Recruitment.Application.Common;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Persistence.RecruitmentProcess
{
    public interface IRejectionReasonRepository
    {
        Task<PagedResult<RejectionReason>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null);

        Task<List<RejectionReason>> GetByTypeAsync(RejectionReasonType reasonType);
    }
}
