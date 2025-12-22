using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Services.CoreBusiness;

namespace Recruitment.Api.Controllers
{
    [ApiController]
    [Route("api/lookups/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _currencyService.GetAllAsync());
        }
    }
}
