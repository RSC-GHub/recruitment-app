using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Application
{
    public class ApplicationReviewVM
    {
        public int ApplicationId { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public string? Note { get; set; }
        public DateTime? ExpectedFirstDate { get; set; }
        public DateTime? ActualFirstDate { get; set; }
    }
}
