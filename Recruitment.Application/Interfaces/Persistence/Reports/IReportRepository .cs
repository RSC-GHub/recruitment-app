using Recruitment.Domain.Entities.Reports;

namespace Recruitment.Application.Interfaces.Persistence.Reports
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<IEnumerable<Report>> GetActiveAsync();

        Task<IEnumerable<Report>> GetAllWithParametersAsync();
        Task<IEnumerable<Report>> GetActiveWithParametersAsync();
        Task<Report?> GetByIdWithParametersAsync(int id);
    }
}
