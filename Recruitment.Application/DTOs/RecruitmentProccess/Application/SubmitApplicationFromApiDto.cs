using Microsoft.AspNetCore.Http;
using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Application
{
    public class SubmitApplicationFromApiDto
    {
        // Applicant details
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Nationality { get; set; } = null!;
        [Required]
        public string CurrentJob { get; set; } = null!;
        [Required]
        public string CurrentEmployer { get; set; } = null!;
        [Required]
        public decimal CurrentSalary { get; set; }
        [Required]
        public decimal ExpectedSalary { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public string? TargetPosition { get; set; }

        // Optional
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public MilitaryStatus MilitaryStatus { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public EducationDegree EducationDegree { get; set; }
        [Required]
        public short? GraduationYear { get; set; }
        public string? Major { get; set; }
        [Required]
        public string? NoticePeriod { get; set; }
        public string? ExtraCertificate { get; set; }

        [Required]
        public IFormFile? CV { get; set; }  // For CV file upload
        [Required]
        public int VacancyId { get; set; }
    }
}
