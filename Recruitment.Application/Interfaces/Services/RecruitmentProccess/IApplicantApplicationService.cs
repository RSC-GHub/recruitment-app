using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Application;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.RecruitmentProccess
{
    public interface IApplicantApplicationService
    {
        // Assign applicant to a vacancy
        Task AssignApplicantAsync(ApplicationCreateDto dto);

        // Review an application
        Task ReviewApplicationAsync(ApplicationReviewDto dto);

        // Get application by ID (for details)
        Task<ApplicationDetailDto?> GetByIdAsync(int applicationId);

        Task<ApplicationDetailDto?> GetApplicationDetails(int applicationId);

        // Get paged applications (all)
        Task<PagedResult<ApplicationListDto>> GetAllApplicationsAsync(int page, int pageSize, ApplicationStatus? status, string search = null!);

        // Get paged applications by vacancy
        Task<PagedResult<ApplicationListDto>> GetByVacancyIdAsync(int vacancyId, int page, int pageSize, string? search = null);

        // Get paged applications by applicant
        Task<PagedResult<ApplicationListDto>> GetByApplicantIdAsync(int applicantId, int page, int pageSize);

        Task<ApplicationDetailDto?> GetByApplicantAndVacancyAsync(int applicantId, int vacancyId);

        // Get paged applications by status
        Task<PagedResult<ApplicationListDto>> GetByStatusAsync(ApplicationStatus status, int page, int pageSize);

        // Get pending review applications (paged)
        Task<PagedResult<ApplicationListDto>> GetPendingReviewAsync(int page, int pageSize);

        // Count applications for a vacancy
        Task<int> CountByVacancyAsync(int vacancyId);

        // Optional: check if applicant already applied to a vacancy
        Task<bool> HasAppliedAsync(int applicantId, int vacancyId);

        Task<bool> DeleteApplicationAsync(int id);

        Task UpdateApplicationStatusAsync(int applicationId, ApplicationStatus newStatus);
    }
}
