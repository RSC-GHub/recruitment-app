using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Services.CoreBusiness;

namespace Recruitment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacanciesController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacanciesController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVacancyById(int id)
        {
            return Ok(await _vacancyService.GetVacancyByIdAsyncForAPI(id));
        }

        [HttpGet("cards")]
        public async Task<IActionResult> GetVacanciesCards([FromQuery] int shortTextLength = 200)
        {
            var vacancyCards = await _vacancyService.GetVacancyCardsAsync(shortTextLength);

            return Ok(vacancyCards);
        }
    }
}
