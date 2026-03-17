using Microsoft.AspNetCore.Http;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.UserManagement.Applicant
{
    public class ApplicantCreateDto
    {
        // Required
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public int CountryId { get; set; }
        public string City { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string? TargetPosition { get; set; }

        public string CurrentJob { get; set; } = null!;
        public string CurrentEmployer { get; set; } = null!;

        public decimal CurrentSalary { get; set; }
        public decimal ExpectedSalary { get; set; }

        public int CurrencyId { get; set; }

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
        public string? Comment { get; set; } = string.Empty;

        public IFormFile? CV { get; set; }   
    }
}
