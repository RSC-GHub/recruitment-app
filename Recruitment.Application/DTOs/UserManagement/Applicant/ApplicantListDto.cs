using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.UserManagement.Applicant
{
    public class ApplicantListDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string CountryName { get; set; } = null!;

        public EducationDegree EducationDegree { get; set; }
        public short? GraduationYear { get; set; }
    }

}
