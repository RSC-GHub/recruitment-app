using Recruitment.Application.Common;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Application.Interfaces.Persistence.RecruitmentProcess
{
    public interface IInterviewerRepository
    {
        Task<PagedResult<Interviewer>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null);
    }
}
