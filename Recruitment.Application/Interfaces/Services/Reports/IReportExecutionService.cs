using System.Data;

namespace Recruitment.Application.Interfaces.Services.Reports
{
    public interface IReportExecutionService
    {
        Task<DataTable> ExecuteAsync(
        int reportId, Dictionary<string, object?> parameters);

    }
}
