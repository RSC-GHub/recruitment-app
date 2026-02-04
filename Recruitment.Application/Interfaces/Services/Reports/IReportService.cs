using Recruitment.Application.DTOs.Reports;

namespace Recruitment.Application.Interfaces.Services.Reports
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllAsync();
        Task<ReportDto?> GetByIdAsync(int id);
        Task<IEnumerable<ReportDto>> GetActiveReportsAsync();
        Task<int> CreateAsync(CreateReportDto dto);
        Task UpdateAsync(UpdateReportDto dto);
        Task DeleteAsync(int id);
    }

}
