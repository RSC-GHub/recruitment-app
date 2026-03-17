using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.UserManagement.Applicant
{
    public class ApplicantEditVM : IApplicantDropdowns
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        [StringLength(20)]
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

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public EducationDegree EducationDegree { get; set; }

        public string? NoticePeriod { get; set; }
        public string? Address { get; set; }
        public short? GraduationYear { get; set; }
        public string? Major { get; set; }
        public string? ExtraCertificate { get; set; }
        public MilitaryStatus MilitaryStatus { get; set; }
        public MaritalStatus MaritalStatus { get; set; }

        public IFormFile? CV { get; set; }  
        public string? ExistingCVPath { get; set; }
        public string? Comment { get; set; } = string.Empty;

        public IEnumerable<SelectListItem>? Countries { get; set; }
        public IEnumerable<SelectListItem>? Currencies { get; set; }
        public IEnumerable<SelectListItem>? GenderType { get; set; }
        public IEnumerable<SelectListItem>? MilitaryStatuses { get; set; }
        public IEnumerable<SelectListItem>? MaritalStatuses { get; set; }
        public IEnumerable<SelectListItem>? EducationDegrees { get; set; }
    }

}
