using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Interviewer;

namespace Recruitment.Application.Interfaces.Services.RecruitmentProccess
{
    public interface IInterviewerService
    {
        Task<List<InterviewerListDTO>> GetAllAsync();
        Task AddAsync(InterviewerCreateDTO dto);
        Task UpdateAsync(InterviewerUpdateDTO dto);
        Task DeleteAsync(int id);

        Task<InterviewerDetailsDTO?> GetByIdAsync(int id);

        Task<PagedResult<InterviewerListDTO>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null);
    }
}
