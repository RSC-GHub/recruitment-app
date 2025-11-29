using Recruitment.Application.Common;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.File;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Application.Services.UserManagement
{
    public class ApplicantService : IApplicantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        public ApplicantService(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
        }

        public async Task<int> CreateApplicantAsync(ApplicantCreateDto dto)
        {
            var applicant = new Applicant
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CountryId = dto.CountryId,
                City = dto.City,
                Nationality = dto.Nationality,
                CurrentJob = dto.CurrentJob,
                CurrentEmployer = dto.CurrentEmployer,
                CurrentSalary = dto.CurrentSalary,
                ExpectedSalary = dto.ExpectedSalary,
                CurrencyId = dto.CurrencyId,
                Address = dto.Address,
                Gender = dto.Gender,
                MilitaryStatus = dto.MilitaryStatus,
                EducationDegree = dto.EducationDegree,
                GraduationYear = dto.GraduationYear,
                Major = dto.Major,
                NoticePeriod = dto.NoticePeriod,
                ExtraCertificate = dto.ExtraCertificate
            };

            if (dto.CV != null)
            {
                applicant.CVFilePath = await _fileStorageService.SaveCVAsync(dto.CV);
            }

            await _unitOfWork.ApplicantRepository.AddAsync(applicant);
            await _unitOfWork.CompleteAsync();

            return applicant.Id;
        }

        public async Task<bool> UpdateApplicantAsync(ApplicantUpdateDto dto)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(dto.Id);
            if (applicant == null) return false;

            applicant.FullName = dto.FullName;
            applicant.Email = dto.Email;
            applicant.PhoneNumber = dto.PhoneNumber;
            applicant.CountryId = dto.CountryId;
            applicant.City = dto.City;
            applicant.Nationality = dto.Nationality;
            applicant.CurrentJob = dto.CurrentJob;
            applicant.CurrentEmployer = dto.CurrentEmployer;
            applicant.CurrentSalary = dto.CurrentSalary;
            applicant.ExpectedSalary = dto.ExpectedSalary;
            applicant.CurrencyId = dto.CurrencyId;
            applicant.Address = dto.Address;
            applicant.Gender = dto.Gender;
            applicant.MilitaryStatus = dto.MilitaryStatus;
            applicant.EducationDegree = dto.EducationDegree;
            applicant.GraduationYear = dto.GraduationYear;
            applicant.Major = dto.Major;
            applicant.NoticePeriod = dto.NoticePeriod;
            applicant.ExtraCertificate = dto.ExtraCertificate;

            if (dto.CV != null)
            {
                if (!string.IsNullOrWhiteSpace(applicant.CVFilePath))
                    _fileStorageService.DeleteFile(applicant.CVFilePath);

                applicant.CVFilePath = await _fileStorageService.SaveCVAsync(dto.CV);
            }

            _unitOfWork.ApplicantRepository.Update(applicant);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<ApplicantUpdateDto?> GetApplicantByIdAsync(int id)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetApplicantProfileAsync(id);
            if (applicant == null) return null;

            return new ApplicantUpdateDto
            {
                Id = applicant.Id,
                FullName = applicant.FullName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                CountryId = applicant.CountryId,
                City = applicant.City,
                Nationality = applicant.Nationality,
                CurrentJob = applicant.CurrentJob,
                CurrentEmployer = applicant.CurrentEmployer,
                CurrentSalary = applicant.CurrentSalary,
                ExpectedSalary = applicant.ExpectedSalary,
                CurrencyId = applicant.CurrencyId,
                Address = applicant.Address,
                Gender = applicant.Gender,
                MilitaryStatus = applicant.MilitaryStatus,
                EducationDegree = applicant.EducationDegree,
                GraduationYear = applicant.GraduationYear,
                Major = applicant.Major,
                NoticePeriod = applicant.NoticePeriod,
                ExtraCertificate = applicant.ExtraCertificate,
                CVFilePath = applicant.CVFilePath
            };
        }

        public async Task<ApplicantProfileDto?> GetApplicantProfileAsync(int id)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetApplicantProfileAsync(id);
            if (applicant == null) return null;

            return new ApplicantProfileDto
            {
                Id = applicant.Id,
                FullName = applicant.FullName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,

                CountryId = applicant.CountryId,
                CountryName = applicant.Country.Name,
                City = applicant.City,
                Nationality = applicant.Nationality,

                CurrentJob = applicant.CurrentJob,
                CurrentEmployer = applicant.CurrentEmployer,

                CurrentSalary = applicant.CurrentSalary,
                ExpectedSalary = applicant.ExpectedSalary,

                CurrencyId = applicant.CurrencyId,
                CurrencyName = applicant.Currency.Name,

                Address = applicant.Address,
                Gender = applicant.Gender,
                MilitaryStatus = applicant.MilitaryStatus,
                EducationDegree = applicant.EducationDegree,
                GraduationYear = applicant.GraduationYear,
                Major = applicant.Major,
                NoticePeriod = applicant.NoticePeriod,
                ExtraCertificate = applicant.ExtraCertificate,
                CVFilePath = applicant.CVFilePath,

                CreatedBy = applicant.CreatedBy,
                CreatedOn = applicant.CreatedOn,
                ModifiedBy = applicant.ModifiedBy,
                ModifiedOn = applicant.ModifiedOn
            };
        }

        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(id);
            if (applicant == null) return false;

            if (!string.IsNullOrWhiteSpace(applicant.CVFilePath))
                _fileStorageService.DeleteFile(applicant.CVFilePath);

            _unitOfWork.ApplicantRepository.Delete(applicant);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<PagedResult<ApplicantListDto>> GetPagedApplicantsAsync(int page, int pageSize, string? search)
        {
            var pagedResult = await _unitOfWork.ApplicantRepository.GetPagedApplicantsAsync(page, pageSize, search);

            var dtoItems = pagedResult.Items.Select(a => new ApplicantListDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                CountryName = a.Country.Name,
                EducationDegree = a.EducationDegree,
                GraduationYear = a.GraduationYear
            }).ToList();

            return new PagedResult<ApplicantListDto>(dtoItems, pagedResult.TotalCount, pagedResult.Page, pagedResult.PageSize);
        }
    }
}
