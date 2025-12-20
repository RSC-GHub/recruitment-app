using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class InterviewListDTO
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string VacancyTitle { get; set; } = null!;
        public string InterviewerName{ get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public InterviewCategory InterviewCategory { get; set; }
        public InterviewStatus InterviewStatus { get; set; }
        public InterviewResult InterviewResult { get; set; }
        public string? InterViewNote { get; set; }
    }
}
