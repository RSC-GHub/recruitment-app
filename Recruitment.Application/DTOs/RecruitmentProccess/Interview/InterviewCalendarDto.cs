namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class InterviewCalendarDto
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; } 
        public string ApplicantName { get; set; } = string.Empty;
        public string VacancyTitle { get; set; } = string.Empty;
    }

}
