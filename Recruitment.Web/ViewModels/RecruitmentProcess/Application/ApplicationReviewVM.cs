using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Application
{
    public class ApplicationReviewVM
    {
        public int ApplicationId { get; set; }
        // REMOVED: public int ReviewedBy { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public string? Note { get; set; }
    }
}
