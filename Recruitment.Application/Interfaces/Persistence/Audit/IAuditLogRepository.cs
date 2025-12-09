using Recruitment.Domain.Entities.Aduit;

namespace Recruitment.Application.Interfaces.Persistence.Audit
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetRecentAsync(int take = 10);
    }

}
