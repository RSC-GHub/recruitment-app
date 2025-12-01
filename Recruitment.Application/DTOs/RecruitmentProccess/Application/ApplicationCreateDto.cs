namespace Recruitment.Application.DTOs.RecruitmentProccess.Application
{
    public class ApplicationCreateDto
    {
        public int ApplicantId { get; set; }
        public int VacancyId { get; set; }
        public string? Note { get; set; }

    }
}
