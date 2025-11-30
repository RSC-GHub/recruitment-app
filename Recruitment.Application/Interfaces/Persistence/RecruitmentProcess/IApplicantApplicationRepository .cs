using Recruitment.Application.Common;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Interfaces.Persistence.RecruitmentProcess
{
    public interface IApplicantApplicationRepository : IGenericRepository<ApplicantApplication>
    {

        Task<PagedResult<ApplicantApplication>> GetByVacancyIdAsync(int vacancyId, int page, int pageSize, string? search = null);
        Task<PagedResult<ApplicantApplication>> GetByApplicantIdAsync(int applicantId, int page, int pageSize);
        Task<ApplicantApplication?> GetByApplicantAndVacancyAsync(int applicantId, int vacancyId);
        Task<ApplicantApplication?> GetApplicationWithRelatedData(int id);
        Task<PagedResult<ApplicantApplication>> GetAllWithDetailsAsync(int page, int pageSize, ApplicationStatus? status, string? search = null!);
        Task<PagedResult<ApplicantApplication>> GetByStatusAsync(ApplicationStatus status, int page, int pageSize);
        Task<PagedResult<ApplicantApplication>> GetPendingReviewAsync(int page, int pageSize);

        Task<int> CountByVacancyAsync(int vacancyId);
        Task AssignApplicantAsync(int applicantId, int vacancyId, string Note);
        Task ReviewApplicationAsync(int applicationId, int reviewedByUserId, ApplicationStatus status, string? note = null);
    }
}
