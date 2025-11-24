using Recruitment.Application.DTOs.CoreBusiness.Vacancy;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyIndexVM
    {
        // Table List
        public List<VacancyListDto> Vacancies { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    }
}
