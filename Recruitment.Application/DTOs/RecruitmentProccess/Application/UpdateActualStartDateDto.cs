namespace Recruitment.Application.DTOs.RecruitmentProccess.Application
{
    public class UpdateActualStartDateDto
    {
        public int ApplicationId { get; set; }
        public DateTime? ActualFirstDate { get; set; }
    }
}
