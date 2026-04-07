using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.RecruitmentProccess.Application;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;

namespace Recruitment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicantApplicationService _applicantApplicationService;
        public ApplicationController(IApplicantApplicationService applicantApplicationService)
        {
            _applicantApplicationService = applicantApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication([FromForm] SubmitApplicationFromApiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var application = await _applicantApplicationService.SubmitApplicationFromAPIAsync(dto);

            return Ok(new
            {
                Message = "Application submitted successfully",
                application.Id,
                application.ApplicationDate,
                application.ApplicationStatus,
                Applicant = new { application.Applicant.FullName, application.Applicant.Email }
            });
        }
    }
}