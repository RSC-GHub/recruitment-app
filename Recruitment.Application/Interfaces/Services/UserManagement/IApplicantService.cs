using Recruitment.Application.Common;
using Recruitment.Application.DTOs.UserManagement.Applicant;

namespace Recruitment.Application.Interfaces.Services.UserManagement
{
    public interface IApplicantService
    {
        Task<PagedResult<ApplicantListDto>> GetPagedApplicantsAsync(int page, int pageSize, string? search);
        Task<ApplicantUpdateDto?> GetApplicantByIdAsync(int id);
        Task<ApplicantProfileDto?> GetApplicantProfileAsync(int id);
        Task<int> CreateApplicantAsync(ApplicantCreateDto dto);
        Task<int> CreateApplicantFromAPIAsync(ApplicantCreateFromAPIDto dto);
        Task<bool> UpdateApplicantAsync(ApplicantUpdateDto dto);
        Task<bool> DeleteApplicantAsync(int id);
        Task<List<ApplicantDropdownDto>> GetAvailableApplicantsForVacancyAsync(int vacancyId);
        Task<List<ApplicantDropdownDto>> GetAllApplicantsAsync();
        Task<ApplicantHistoryDto?> GetApplicantHistoryAsync(int applicantId);
        Task<List<ApplicantListDto>> GetDuplicateApplicantsAsync(int applicantId);
        Task<List<ApplicantDuplicateDto>> GetApplicantDuplicatesWithOwnerInfoAsync(int applicantId);
    }
}
