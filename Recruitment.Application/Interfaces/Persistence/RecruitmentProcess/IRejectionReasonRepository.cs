using Recruitment.Application.Common;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Application.Interfaces.Persistence.RecruitmentProcess
{
    public interface IRejectionReasonRepository
    {
        Task<PagedResult<RejectionReason>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null);
    }
}
