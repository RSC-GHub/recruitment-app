using Recruitment.Application.Common;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Application.Interfaces.Persistence.UserManagement
{
    public interface IApplicantRepository : IGenericRepository<Applicant>
    {
        Task<Applicant?> GetApplicantProfileAsync(int applicantId);
        Task<PagedResult<Applicant>> GetPagedApplicantsAsync(int page, int pageSize, string? search = null);
        Task<List<Applicant>> GetApplicantsWithHistoryAsync(int applicantId);
    }
}
