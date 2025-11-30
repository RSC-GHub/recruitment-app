using Recruitment.Application.DTOs.RecruitmentProccess;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyApplicationsVM
    {
        public int VacancyId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        //serach
        public string? Search { get; set; }
        public IEnumerable<ApplicationListDto> Applications { get; set; } = new List<ApplicationListDto>();
    }

}
