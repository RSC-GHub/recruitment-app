using Recruitment.Application.DTOs.RecruitmentProccess.Interview;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interview
{
    public class InterviewIndexVM
    {
        public string? Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<InterviewListDTO> Interviews { get; set; } = new();
    }

}
