using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Services.RecruitmentProccess
{
    public class ApplicantApplicationService : IApplicantApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;


        public ApplicantApplicationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private async Task<int?> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            return user?.Id;
        }

        public async Task AssignApplicantAsync(ApplicationCreateDto dto)
        {
            var exists = await _unitOfWork.ApplicationRepository.GetByApplicantAndVacancyAsync(dto.ApplicantId, dto.VacancyId);
            if (exists != null)
                throw new InvalidOperationException("Applicant already assigned to this vacancy.");

            // Get current user id
            var currentUserId = await GetCurrentUserIdAsync();
            if (currentUserId == null)
                throw new InvalidOperationException("Unable to determine current user.");


            await _unitOfWork.ApplicationRepository.AssignApplicantAsync(
                dto.ApplicantId,
                dto.VacancyId,
                currentUserId.Value,
                dto.Note!
            );

            await _unitOfWork.CompleteAsync();
        }

        // Review an application
        public async Task ReviewApplicationAsync(ApplicationReviewDto dto)
        {
            await _unitOfWork.ApplicationRepository.ReviewApplicationAsync(dto.ApplicationId, dto.ReviewedBy, dto.ApplicationStatus, dto.Note);
            await _unitOfWork.CompleteAsync();
        }

        // Get application by ID (details)
        public async Task<ApplicationDetailDto?> GetByIdAsync(int applicationId)
        {
            var entity = await _unitOfWork.ApplicationRepository.GetByIdAsync(applicationId);
            if (entity == null) return null;

            return new ApplicationDetailDto
            {
                Id = entity.Id,

                ApplicantId = entity.ApplicantId,
                ApplicantName = entity.Applicant?.FullName ?? "",
                ApplicantEmail = entity.Applicant?.Email ?? "",
                PhoneNumber = entity.Applicant?.PhoneNumber ?? "",
                CurrentJob = entity.Applicant?.CurrentJob,
                CurrentEmployer = entity.Applicant?.CurrentEmployer,

                VacancyId = entity.VacancyId,
                VacancyTitle = entity.Vacancy?.Title!.Name ?? "",
                VacancyDescription = entity.Vacancy?.JobDescription,

                ApplicationStatus = entity.ApplicationStatus,
                ApplicationDate = entity.ApplicationDate,

                ReviewedBy = entity.ReviewedBy,
                ReviewedByUserName = entity.User?.FullName,
                ReviewDate = entity.ReviewDate,
                Note = entity.Note
            };
        }

        // Map ApplicantApplication to ApplicationListDto
        private PagedResult<ApplicationListDto> MapToPagedResult(PagedResult<ApplicantApplication> paged)
        {
            var items = paged.Items.Select(a => new ApplicationListDto
            {
                Id = a.Id,
                ApplicantId = a.ApplicantId,
                ApplicantName = a.Applicant?.FullName ?? "",
                ApplicantEmail = a.Applicant?.Email ?? "",
                PhoneNumber = a.Applicant?.PhoneNumber ?? "",

                VacancyId = a.VacancyId,
                VacancyTitle = a.Vacancy?.Title!.Name ?? "",

                ApplicationStatus = a.ApplicationStatus,
                ApplicationDate = a.ApplicationDate,

                ReviewedByUserName = a.User?.FullName,
                ReviewDate = a.ReviewDate,
                Note = a.Note
            }).ToList();

            return new PagedResult<ApplicationListDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
        }

        // Get all applications (paged)
        public async Task<PagedResult<ApplicationListDto>> GetAllApplicationsAsync(int page, int pageSize)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetAllWithDetailsAsync(page, pageSize);
            return MapToPagedResult(paged);
        }

        // Get applications by vacancy (paged)
        public async Task<PagedResult<ApplicationListDto>> GetByVacancyIdAsync(int vacancyId, int page, int pageSize)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetByVacancyIdAsync(vacancyId, page, pageSize);
            return MapToPagedResult(paged);
        }

        // Get applications by applicant (paged)
        public async Task<PagedResult<ApplicationListDto>> GetByApplicantIdAsync(int applicantId, int page, int pageSize)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetByApplicantIdAsync(applicantId, page, pageSize);
            return MapToPagedResult(paged);
        }

        // Get applications by status (paged)
        public async Task<PagedResult<ApplicationListDto>> GetByStatusAsync(ApplicationStatus status, int page, int pageSize)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetByStatusAsync(status, page, pageSize);
            return MapToPagedResult(paged);
        }

        // Get pending review applications (paged)
        public async Task<PagedResult<ApplicationListDto>> GetPendingReviewAsync(int page, int pageSize)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetPendingReviewAsync(page, pageSize);
            return MapToPagedResult(paged);
        }

        // Count applications for a vacancy
        public async Task<int> CountByVacancyAsync(int vacancyId)
        {
            return await _unitOfWork.ApplicationRepository.CountByVacancyAsync(vacancyId);
        }

        // Check if applicant already applied
        public async Task<bool> HasAppliedAsync(int applicantId, int vacancyId)
        {
            var entity = await _unitOfWork.ApplicationRepository.GetByApplicantAndVacancyAsync(applicantId, vacancyId);
            return entity != null;
        }
    }
}
