using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.UserManagement.Applicant
{
    public class ApplicantProfileVM
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Nationality { get; set; } = null!;

        public string? TargetPosition { get; set; }
        public string CurrentJob { get; set; } = null!;
        public string CurrentEmployer { get; set; } = null!;

        public decimal CurrentSalary { get; set; }
        public decimal ExpectedSalary { get; set; }

        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; } = null!;

        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public MilitaryStatus MilitaryStatus { get; set; }
        public MaritalStatus MaritalStatus { get; set; } 
        public EducationDegree EducationDegree { get; set; }
        public short? GraduationYear { get; set; }
        public string? Major { get; set; }
        public string? NoticePeriod { get; set; }
        public string? ExtraCertificate { get; set; }

        public string CVFilePath { get; set; } = null!;

        // Audit
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
