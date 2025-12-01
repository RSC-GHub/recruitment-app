using Recruitment.Application.Common;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Persistence.RecruitmentProcess
{
    public interface IInterviewRepository : IGenericRepository<Interview>
    {
        Task<PagedResult<Interview>> GetPagedAsync(int page, int pageSize);

        Task<PagedResult<Interview>> GetByApplicationIdAsync(
            int applicationId, int page, int pageSize);

        Task<PagedResult<Interview>> SearchAsync(
            string? interviewer,
            InterviewStatus? status,
            InterviewResult? result,
            int page,
            int pageSize);

        Task<Interview?> GetWithApplicationAsync(int id);

        Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback);
    Task<bool> UpdateInterviewStatusAsync(int id, InterviewStatus status);
    }
}
