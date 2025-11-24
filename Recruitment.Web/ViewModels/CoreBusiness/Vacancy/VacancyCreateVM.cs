using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyCreateVM
    {
        public VacancyCreateDto Vacancy { get; set; } = new VacancyCreateDto();

        public List<TitleDto> Titles { get; set; } = new List<TitleDto>();
    }
}