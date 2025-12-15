using Recruitment.Application.DTOs.UserManagement.Applicant;

namespace Recruitment.Application.Interfaces.Common
{
    public interface IExcelExportService
    {
        byte[] ExportApplicants(List<ApplicantExportDto> data);
    }

}
