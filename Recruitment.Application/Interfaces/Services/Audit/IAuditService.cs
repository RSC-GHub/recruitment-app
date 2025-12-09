using Recruitment.Application.DTOs.Audit;

namespace Recruitment.Application.Interfaces.Services.Audit
{
    public interface IAuditService
    {
        Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int take = 10);
    }
}
