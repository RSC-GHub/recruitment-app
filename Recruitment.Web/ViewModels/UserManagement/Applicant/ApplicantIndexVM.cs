using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.UserManagement.Applicant
{
    public class ApplicantIndexVM
    {
        public IEnumerable<ApplicantListVM> Applicants { get; set; } = new List<ApplicantListVM>();

        // Paging
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // Search
        public string? Search { get; set; }
    }

    public class ApplicantListVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public EducationDegree EducationDegree { get; set; }
        public short? GraduationYear { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public string? OfferStatus { get; set; } = string.Empty;

    }
}
