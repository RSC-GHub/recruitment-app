using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.RecruitmentProccess
{
    public interface IInterviewService
    {
        Task<PagedResult<InterviewListDTO>> GetPagedAsync(int page, int pageSize, string? search = null);
        Task<PagedResult<InterviewListDTO>> GetByApplicationIdAsync(int applicationId, int page, int pageSize);
        Task<PagedResult<InterviewListDTO>> SearchAsync(
            string? interviewer,
            InterviewStatus? status,
            InterviewResult? result,
            int page,
            int pageSize);

        Task<InterviewDetailDTO?> GetByIdAsync(int id);

        Task<bool> CreateAsync(InterviewCreateUpdateDTO dto);
        Task<bool> UpdateAsync(int id, InterviewCreateUpdateDTO dto);

        Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback);
        Task<bool> UpdateInterviewStatusAsync(int id, InterviewStatus status);
    }

}
