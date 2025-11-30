using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Application
{
    public class ApplicationIndexVM
    {
        public string? Search { get; set; }
        public ApplicationStatus? Status { get; set; }

        public List<SelectListItem> StatusList { get; set; } = new();

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public List<ApplicationListDto> Applications { get; set; } = new();
    }


}
