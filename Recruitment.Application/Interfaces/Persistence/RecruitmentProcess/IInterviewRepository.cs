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
        Task<Interview> GetWithApplicationIdAsync(int id);
        Task<List<Interview>> GetAllByApplicationIdAsync(int applicationId);
        Task<PagedResult<Interview>> SearchAsync(
            string? search,
            InterviewStatus? status,
            InterviewResult? result,
            InterviewType? type,
            InterviewCategory? category,
            DateTime? fromDate,
            DateTime? toDate,
            int? interviewerId,
            int page,
            int pageSize);
        Task<Interview?> GetWithRejectionReasonsAsync(int id);
        Task<Interview?> GetWithApplicationAsync(int id);
        Task<Interview?> GetWithRelatedDataAsync(int id);

        Task<bool> UpdateInterviewResultAsync(
            int id,
            InterviewResult result,
            string? feedback,
            string? note,
            List<int>? rejectionReasonIds = null);
        Task<bool> UpdateInterviewAsync(int id, int interviewerId,
                                                InterviewStatus interviewStatus,
                                                InterviewType interviewType,
                                                int durationMinutes, string? interviewNote);

        Task<int> CountTodaysInterviewsAsync();
        Task<int> CountPendingInterviewResultsAsync();

        Task<Interview?> GetByIdWithApplicantAsync(int id);
        Task<List<Interview>> GetAllForCalendarAsync(int? month = null, int? year = null);
        Task<bool> HasOverlappingInterviewAsync(
    int interviewerId,
    DateTime start,
    DateTime end);



    }
}
