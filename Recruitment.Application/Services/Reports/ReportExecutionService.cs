using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.Reports;
using System.Data;

namespace Recruitment.Application.Services.Reports
{
    public class ReportExecutionService : IReportExecutionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportExecutor _executor;

        public ReportExecutionService(
            IUnitOfWork unitOfWork,
            IReportExecutor executor)
        {
            _unitOfWork = unitOfWork;
            _executor = executor;
        }

        public async Task<DataTable> ExecuteAsync(
            int reportId,
            Dictionary<string, object?> parameters)
        {
            var report = await _unitOfWork.ReportsRepository.GetByIdAsync(reportId);

            if (report == null)
                throw new Exception("Report not found");

            if (!report.IsActive)
                throw new Exception("Report is inactive");

            return await _executor.ExecuteAsync(
                report.StoredProcedure,
                parameters
            );
        }
    }
}
