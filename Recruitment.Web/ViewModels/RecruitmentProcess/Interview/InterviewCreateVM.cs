using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interview
{
    namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interview
    {
        public class InterviewCreateVM
        {
            public int ApplicationId { get; set; }  
            public string? Interviewer { get; set; } 
            public DateTime ScheduledDate { get; set; } = DateTime.Now;
            public InterviewType InterviewType { get; set; } = InterviewType.Technical;
            public int DurationMinutes { get; set; } = 60;
            public string? InterViewNote { get; set; }
        }
    }

}
