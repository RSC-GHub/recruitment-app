using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.RecruitmentProccess
{
    public interface IInterviewService
    {
        Task<PagedResult<InterviewListDTO>> GetPagedAsync(
                        int page,
                        int pageSize,
                        string? search,
                        InterviewStatus? status,
                        InterviewResult? result,
                        InterviewType? type,
                        InterviewCategory? category,
                        DateTime? fromDate,
                        DateTime? toDate);
        Task<PagedResult<InterviewListDTO>> GetByApplicationIdAsync(int applicationId, int page, int pageSize);
        Task<PagedResult<InterviewListDTO>> SearchAsync(
            string? search,
            InterviewStatus? status,
            InterviewResult? result,
            InterviewType? type,
            InterviewCategory? category,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize);

        Task<InterviewDetailDTO?> GetByIdAsync(int id);

        Task<bool> CreateAsync(InterviewCreateUpdateDTO dto);
        Task<bool> UpdateAsync(int id, InterviewCreateUpdateDTO dto);

        Task<int?> UpdateInterviewResultAsync(
            int interviewId,
            InterviewResult result,
            string? feedback,
            string? note);
        Task<bool> UpdateInterviewAsync(UpdateInterviewDTO dto);

        Task<int> CountTodaysInterviewsAsync();
        Task<int> GetPendingInterviewResultsAlertAsync();

        Task<List<InterviewCalendarDto>> GetInterviewsForCalendarAsync(int? month = null, int? year = null);

    }

}
