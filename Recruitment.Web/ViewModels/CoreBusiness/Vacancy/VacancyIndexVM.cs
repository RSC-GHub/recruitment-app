using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyIndexVM
    {
        public string? Search { get; set; }
        public int? TitleId { get; set; }
        public int? ProjectId { get; set; }
        public VacancyStatus? Status { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public List<VacancyListDTO> Vacancies { get; set; } = new();

        public IEnumerable<SelectListItem>? Titles { get; set; }
        public IEnumerable<SelectListItem>? Projects { get; set; }
        public IEnumerable<SelectListItem>? Statuses { get; set; }
    }


}
