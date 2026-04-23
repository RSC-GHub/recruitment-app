using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.UserManagement.Applicant
{
    public class ApplicantCreateVM : IApplicantDropdowns
    {
        // Required Fields
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(100, ErrorMessage = "Phone Number cannot exceed 30 characters")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        [Display(Name = "City")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Nationality is required")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters")]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; } = null!;

        public string? TargetPosition { get; set; } 

        [Required(ErrorMessage = "Current Job is required")]
        [StringLength(100, ErrorMessage = "Current Job cannot exceed 100 characters")]
        [Display(Name = "Current Job")]
        public string CurrentJob { get; set; } = null!;

        [Required(ErrorMessage = "Current Employer is required")]
        [StringLength(100, ErrorMessage = "Current Employer cannot exceed 100 characters")]
        [Display(Name = "Current Employer")]
        public string CurrentEmployer { get; set; } = null!;

        [Required(ErrorMessage = "Current Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Current Salary must be a positive value")]
        [Display(Name = "Current Salary")]
        public decimal CurrentSalary { get; set; }

        [Required(ErrorMessage = "Expected Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Expected Salary must be a positive value")]
        [Display(Name = "Expected Salary")]
        public decimal ExpectedSalary { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Education Degree is required")]
        [Display(Name = "Education Degree")]
        public EducationDegree EducationDegree { get; set; }

        [Required(ErrorMessage = "Notice Period is required")]
        [StringLength(50, ErrorMessage = "Notice Period cannot exceed 50 characters")]
        [Display(Name = "Notice Period (days)")]
        public string? NoticePeriod { get; set; }

        [Required(ErrorMessage = "CV is required")]
        [Display(Name = "Upload CV")]
        public IFormFile? CV { get; set; }

        // Conditional Required - Only for Males
        [Display(Name = "Military Status")]
        public MilitaryStatus MilitaryStatus { get; set; }

        [Required(ErrorMessage = "Marital status is required")]
        [Display(Name = "Marital Status")]
        public MaritalStatus MaritalStatus { get; set; } 

        // Optional Fields
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Range(1950, 2100, ErrorMessage = "Graduation Year must be between 1950 and 2100")]
        [Display(Name = "Graduation Year")]
        public short? GraduationYear { get; set; }

        [StringLength(100, ErrorMessage = "Major cannot exceed 100 characters")]
        [Display(Name = "Major")]
        public string? Major { get; set; }

        [StringLength(500, ErrorMessage = "Extra Certificate cannot exceed 500 characters")]
        [Display(Name = "Extra Certificate")]
        public string? ExtraCertificate { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public string? OfferStatus { get; set; } = string.Empty;

        // Dropdowns
        public IEnumerable<SelectListItem>? Countries { get; set; }
        public IEnumerable<SelectListItem>? Currencies { get; set; }
        public IEnumerable<SelectListItem>? MilitaryStatuses { get; set; } = null!;
        public IEnumerable<SelectListItem>? MaritalStatuses { get; set; } = null!;
        public IEnumerable<SelectListItem>? EducationDegrees { get; set; } = null!;
        public IEnumerable<SelectListItem>? GenderType { get; set; } = null!;
    }
}