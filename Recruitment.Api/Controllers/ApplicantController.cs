using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Application.Interfaces.Services.UserManagement;

namespace Recruitment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateApplicant([FromForm] ApplicantCreateFromAPIDto dto)
        {
            var createDto = new ApplicantCreateFromAPIDto
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CountryId = dto.CountryId,
                City = dto.City,
                Nationality = dto.Nationality,
                TargetPosition = dto.TargetPosition,
                CurrentJob = dto.CurrentJob,
                CurrentEmployer = dto.CurrentEmployer,
                CurrentSalary = dto.CurrentSalary,
                ExpectedSalary = dto.ExpectedSalary,
                CurrencyId = dto.CurrencyId,
                Address = dto.Address,
                Gender = dto.Gender,
                MilitaryStatus = dto.MilitaryStatus,
                MaritalStatus = dto.MaritalStatus,
                EducationDegree = dto.EducationDegree,
                GraduationYear = dto.GraduationYear,
                Major = dto.Major,
                NoticePeriod = dto.NoticePeriod,
                ExtraCertificate = dto.ExtraCertificate,
                CV = dto.CV,
            };

            var applicantId = await _applicantService.CreateApplicantFromAPIAsync(createDto);

            return Ok(new
            {
                Message = "Applicant created successfully",
                ApplicantId = applicantId
            });
        }
    }
}
