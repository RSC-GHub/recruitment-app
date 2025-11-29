namespace Recruitment.Application.DTOs.UserManagement.Applicant
{
    public class ApplicantDropdownDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CurrentJob { get; set; } = null!; 
    }
}
