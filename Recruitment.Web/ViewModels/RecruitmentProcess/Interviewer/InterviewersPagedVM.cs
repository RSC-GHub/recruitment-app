namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interviewer
{
    public class InterviewersPagedVM
    {
        public List<InterviewerListVM> Items { get; set; } = new();

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string? Search { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
