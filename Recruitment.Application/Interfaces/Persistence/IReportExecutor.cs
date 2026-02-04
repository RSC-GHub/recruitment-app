using System.Data;

namespace Recruitment.Application.Interfaces.Persistence
{
    public interface IReportExecutor
    {
        Task<DataTable> ExecuteAsync(
        string storedProcedure,
        Dictionary<string, object?> parameters
    );

    }
}
