using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interview
{
    public class InterviewIndexVM
    {
        // Search text
        public string? Search { get; set; }

        // Filters
        public InterviewStatus? Status { get; set; }
        public InterviewResult? Result { get; set; }
        public InterviewType? Type { get; set; }
        public InterviewCategory? InterviewCategory { get; set; }
        public List<SelectListItem> Interviewers { get; set; } = new();
        public int? InterviewerId { get; set; }
        public DateTime? FromDate { get; set; } = DateTime.MinValue;
        public DateTime? ToDate { get; set; } = DateTime.MaxValue;

        // Pagination
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // Data
        public List<InterviewListDTO> Interviews { get; set; } = new();

        // Dropdowns
        public List<SelectListItem>? InterviewTypes { get; set; }
        public List<SelectListItem>? InterviewResults { get; set; }
    }



}
