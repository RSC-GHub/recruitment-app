using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.UserManagement
{
    public class Applicant : BaseEntity
    {
        // Required
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public int CountryId { get; set; }
        public Country Country { get; set; } = null!;

        public string City { get; set; } = null!;
        public string Nationality { get; set; } = null!;

        public string CurrentJob { get; set; } = null!;
        public string CurrentEmployer { get; set; } = null!;

        public decimal CurrentSalary { get; set; }
        public decimal ExpectedSalary { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;

        // Optional
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

        public ICollection<ApplicantApplication> Applications { get; set; } = new List<ApplicantApplication>();


    }
}
