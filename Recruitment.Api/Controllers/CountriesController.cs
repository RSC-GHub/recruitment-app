using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Services.CoreBusiness;

namespace Recruitment.Api.Controllers
{
    [ApiController]
    [Route("api/lookups/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _countryService.GetAllAsync());
        }
    }

}
