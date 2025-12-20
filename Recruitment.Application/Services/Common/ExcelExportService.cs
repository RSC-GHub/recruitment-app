using ClosedXML.Excel;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Application.Interfaces.Common;

namespace Recruitment.Application.Services.Common
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportApplicants(List<ApplicantExportDto> data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Applicants");

            // ===== Header =====
            var headers = new[]
            {
                "Name",
                "Email ID",
                "Contact Number",
                "Major",
                "Graduation Year",
                "Position",
                "Current Position",
                "Current Company",
                "Project",
                "Department",
                "Tech Result",
                "Proposed Salary",
                "Offer Status",
                "N.B",
                "Starting Date",
                "Total Experience",
                "Current Salary",
                "Expected Salary",
                "Reason",
                "Notice Period",
                "relatives",
                "ID",
                "ID's Location/Address",
                "HR Interview Date",
                "Recruiter Name",
                "HR Result",
                "HR Comments",
                "Tech Note",
                "HR Interviewer",
                "Tech Interviewer",
                "Offer negotiations details",
                "Tech Interview Date"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cell(1, i + 1).Value = headers[i];
                ws.Cell(1, i + 1).Style.Font.Bold = true;
            }

            // ===== Data =====
            for (int i = 0; i < data.Count; i++)
            {
                var r = i + 2;
                var d = data[i];

                ws.Cell(r, 1).Value = d.ApplicantName;
                ws.Cell(r, 2).Value = d.Email;
                ws.Cell(r, 3).Value = d.ContactNumber;
                ws.Cell(r, 4).Value = d.Major;
                ws.Cell(r, 5).Value = d.GraduationYear;

                ws.Cell(r, 6).Value = d.Position;
                ws.Cell(r, 7).Value = d.CurrentPosition;
                ws.Cell(r, 8).Value = d.CurrentCompany;

                ws.Cell(r, 9).Value = d.Projects;
                ws.Cell(r, 10).Value = d.Departments;

                ws.Cell(r, 11).Value = d.TechResult?.ToString();
                ws.Cell(r, 12).Value = ""; // Proposed Salary
                ws.Cell(r, 13).Value = ""; // Offer Status
                ws.Cell(r, 14).Value = ""; // N.B
                ws.Cell(r, 15).Value = ""; // Starting Date
                ws.Cell(r, 16).Value = ""; // Total Experience

                ws.Cell(r, 17).Value = d.CurrentSalary;
                ws.Cell(r, 18).Value = d.ExpectedSalary;

                ws.Cell(r, 19).Value = ""; // Reason

                ws.Cell(r, 20).Value = d.NoticePeriod;

                ws.Cell(r, 21).Value = ""; // relatives
                ws.Cell(r, 22).Value = ""; // ID

                ws.Cell(r, 23).Value = d.Address;

                ws.Cell(r, 24).Value = d.HRInterviewDate;

                ws.Cell(r, 25).Value = d.RecruiterName;
                ws.Cell(r, 26).Value = d.HRResult?.ToString();

                ws.Cell(r, 27).Value = d.HRNote;
                ws.Cell(r, 28).Value = d.TechNote;

                ws.Cell(r, 29).Value = d.HRInterviewer;
                ws.Cell(r, 30).Value = d.TechInterviewer;

                ws.Cell(r, 31).Value = ""; // Offer negotiations details
                ws.Cell(r, 32).Value = d.TechInterviewDate;

                //ws.Cell(r, 33).Value = d.CV;
            }

            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
