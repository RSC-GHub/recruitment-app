using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Application;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Services.RecruitmentProccess
{
    public interface IApplicantApplicationService
    {
        Task AssignApplicantAsync(ApplicationCreateDto dto);
        Task ReviewApplicationAsync(ApplicationReviewDto dto);

        Task ChangeStatusAsync(int applicationId, ApplicationStatus newStatus);
        Task ProcessInterviewResultAsync(int applicationId, InterviewResult result);

        Task<ApplicationDetailDto?> GetByIdAsync(int applicationId);
        Task<ApplicationDetailDto?> GetApplicationDetails(int applicationId);

        Task<PagedResult<ApplicationListDto>> GetAllApplicationsAsync(
            int page, int pageSize, ApplicationStatus? status, string search = null!);

        Task<PagedResult<ApplicationListDto>> GetByVacancyIdAsync(
            int vacancyId, int page, int pageSize, string? search = null);

        Task<PagedResult<ApplicationListDto>> GetByApplicantIdAsync(
            int applicantId, int page, int pageSize);

        Task<ApplicationDetailDto?> GetByApplicantAndVacancyAsync(
            int applicantId, int vacancyId);

        Task<PagedResult<ApplicationListDto>> GetByStatusAsync(
            ApplicationStatus status, int page, int pageSize);

        Task<PagedResult<ApplicationListDto>> GetPendingReviewAsync(
            int page, int pageSize);

        Task<int> CountByVacancyAsync(int vacancyId);
        Task<bool> HasAppliedAsync(int applicantId, int vacancyId);
        Task<bool> DeleteApplicationAsync(int id);

        Task<bool> CanMoveToSecondInterviewAsync(int applicationId);
        Task<bool> CanAddInterviewAsync(int applicationId);

        Task<int> CountApplicationsAsync(ApplicationStatus? status = null);

        Task<int> GetOnHoldApplicationsAlertAsync(int days = 3);
        Task<List<ApplicantExportDto>> ExportApplicantsAsync(
            ApplicationStatus? status,
            string? search);

        Task UpdateActualStartDateAsync(UpdateActualStartDateDto dto);

    }

}
