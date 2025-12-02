using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interview
{
    public class UpdateInterviewVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Interviewer")]
        public string? InterViewer { get; set; }

        [Required]
        [Display(Name = "Scheduled Date")]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [Display(Name = "Interview Type")]
        public InterviewType InterviewType { get; set; }

        [Required]
        [Display(Name = "Interview Status")]
        public InterviewStatus InterviewStatus { get; set; }

        [Required]
        [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes.")]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Display(Name = "Interview Note")]
        [MaxLength(1000)]
        public string? InterviewNote { get; set; }

        [Display(Name = "Feedback")]
        [MaxLength(2000)]
        public string? Feedback { get; set; }
    }
}
