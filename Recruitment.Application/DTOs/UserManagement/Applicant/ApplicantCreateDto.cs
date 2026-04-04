using Microsoft.AspNetCore.Http;
using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs.UserManagement.Applicant
{
    public class ApplicantCreateFromAPIDto
    {
        // Required
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

        public string? TargetPosition { get; set; }
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
        public string? Address { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public MilitaryStatus MilitaryStatus { get; set; }
        [Required]
        public MaritalStatus MaritalStatus { get; set; }
        [Required]
        public EducationDegree EducationDegree { get; set; }
        public short? GraduationYear { get; set; }
        public string? Major { get; set; }
        public string? NoticePeriod { get; set; }
        public string? ExtraCertificate { get; set; }
        public IFormFile? CV { get; set; }
    }
    public class ApplicantCreateDto : ApplicantCreateFromAPIDto
    {
        
        public string? OfferStatus { get; set; } = string.Empty;

        public string? Comment { get; set; } = string.Empty;

    }
}
