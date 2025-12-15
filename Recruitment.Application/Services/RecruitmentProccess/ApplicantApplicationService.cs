using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Application;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Workflows;
using System.Data;
using Dapper;

namespace Recruitment.Application.Services.RecruitmentProccess
{
    public class ApplicantApplicationService : IApplicantApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbConnection _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;


        public ApplicantApplicationService(IUnitOfWork unitOfWork, 
            IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, IDbConnection connection)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _connection = connection;
        }

        private async Task<int?> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User!);
            return user?.Id;
        }

        public async Task AssignApplicantAsync(ApplicationCreateDto dto)
        {
            var exists = await _unitOfWork.ApplicationRepository.GetByApplicantAndVacancyAsync(dto.ApplicantId, dto.VacancyId);
            if (exists != null)
                throw new InvalidOperationException("Applicant already assigned to this vacancy.");

            await _unitOfWork.ApplicationRepository.AssignApplicantAsync(
                dto.ApplicantId,
                dto.VacancyId,
                dto.Note!
            );

            await _unitOfWork.CompleteAsync();
        }

        public async Task ChangeStatusAsync(int applicationId, ApplicationStatus newStatus)
        {
            var application = await _unitOfWork.ApplicationRepository
                .GetByIdAsync(applicationId);

            if (application == null)
                throw new InvalidOperationException("Application not found.");

            ApplicationWorkflow.ValidateTransition(
                application.ApplicationStatus,
                newStatus);

            application.ApplicationStatus = newStatus;

            _unitOfWork.ApplicationRepository.Update(application);
            await _unitOfWork.CompleteAsync();
        }


        // Review an application
        public async Task ReviewApplicationAsync(ApplicationReviewDto dto)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId == null)
                throw new InvalidOperationException("Current user not found.");

            var application = await _unitOfWork.ApplicationRepository
                .GetByIdAsync(dto.ApplicationId);

            if (application == null)
                throw new InvalidOperationException("Application not found.");

            ApplicationWorkflow.ValidateTransition(
                application.ApplicationStatus,
                dto.ApplicationStatus);

            application.ApplicationStatus = dto.ApplicationStatus;
            application.ReviewedBy = userId.Value;
            application.ReviewDate = DateTime.UtcNow;
            application.Note = dto.Note;

            _unitOfWork.ApplicationRepository.Update(application);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ProcessInterviewResultAsync(
                int applicationId,
                InterviewResult result)
        {
            var application = await _unitOfWork.ApplicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                throw new InvalidOperationException("Application not found");

            ApplicationStatus nextStatus = result switch
            {
                InterviewResult.Accepted => ApplicationStatus.AcceptedInterview,
                InterviewResult.SecondChoice => ApplicationStatus.OnHold,
                InterviewResult.Rejected => ApplicationStatus.Rejected,
                _ => throw new InvalidOperationException("Invalid interview result")
            };

            ApplicationWorkflow.ValidateTransition(application.ApplicationStatus, nextStatus);

            application.ApplicationStatus = nextStatus;
            _unitOfWork.ApplicationRepository.Update(application);
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
                ReviewedByUserName = entity.Reviewer?.FullName,
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
                VacancyTitle = a.Vacancy?.Title?.Name ?? "",

                ApplicationStatus = a.ApplicationStatus,
                ApplicationDate = a.ApplicationDate,

                ReviewedByUserName = a.Reviewer?.FullName,
                ReviewDate = a.ReviewDate,
                Note = a.Note
            }).ToList();

            return new PagedResult<ApplicationListDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
        }

        // Get all applications (paged)
        public async Task<PagedResult<ApplicationListDto>> GetAllApplicationsAsync(int page, int pageSize, ApplicationStatus? status, string? search)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetAllWithDetailsAsync(page, pageSize, status, search);
            return MapToPagedResult(paged);
        }

        // Get applications by vacancy (paged)
        public async Task<PagedResult<ApplicationListDto>> GetByVacancyIdAsync(
            int vacancyId,
            int page,
            int pageSize,
            string? search = null)
        {
            var paged = await _unitOfWork.ApplicationRepository.GetByVacancyIdAsync(vacancyId, page, pageSize, search);

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

        public async Task<ApplicationDetailDto?> GetByApplicantAndVacancyAsync(int applicantId, int vacancyId)
        {
            var entity = await _unitOfWork.ApplicationRepository.GetByApplicantAndVacancyAsync(applicantId, vacancyId);
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
                ReviewedByUserName = entity.Reviewer?.FullName,
                ReviewDate = entity.ReviewDate,
                Note = entity.Note
            };
        }

        public async Task<ApplicationDetailDto?> GetApplicationDetails(int applicationId)
        {
            var entity = await _unitOfWork.ApplicationRepository.GetApplicationWithRelatedData(applicationId);
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
                CV = entity.Applicant?.CVFilePath ?? "",

                VacancyId = entity.VacancyId,
                VacancyTitle = entity.Vacancy?.Title!.Name ?? "",
                VacancyDescription = entity.Vacancy?.JobDescription,

                ApplicationStatus = entity.ApplicationStatus,
                ApplicationDate = entity.ApplicationDate,

                ReviewedBy = entity.ReviewedBy,
                ReviewedByUserName = entity.Reviewer?.FullName,
                ReviewDate = entity.ReviewDate,
                Note = entity.Note
            };
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _unitOfWork.ApplicationRepository.GetByIdAsync(id);
            if (application == null) return false;

            _unitOfWork.ApplicationRepository.Delete(application);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> CanMoveToSecondInterviewAsync(int applicationId)
        {
            var application = await _unitOfWork.ApplicationRepository
                .GetByIdAsync(applicationId);

            if (application == null)
                return false;

            if (application.ApplicationStatus != ApplicationStatus.Interviewing)
                return false;

            var interviews = await _unitOfWork.InterviewRepository
                .GetAllByApplicationIdAsync(applicationId);

            return interviews.Any(i =>
                i.InterviewStatus == InterviewStatus.Completed &&
                i.InterviewResult == InterviewResult.Accepted);
        }

        public async Task<bool> CanAddInterviewAsync(int applicationId)
        {
            var application = await _unitOfWork.ApplicationRepository
                .GetByIdAsync(applicationId);

            if (application == null)
                return false;

            if (application.ApplicationStatus != ApplicationStatus.Interviewing)
                return false;

            var interviews = await _unitOfWork.InterviewRepository
                .GetAllByApplicationIdAsync(applicationId);

            return !interviews.Any(i =>
                i.InterviewStatus == InterviewStatus.Scheduled);
        }

        public async Task<int> CountApplicationsAsync(ApplicationStatus? status = null)
        {
            return await _unitOfWork.ApplicationRepository.CountByStatusAsync(status);
        }

        public async Task<int> GetOnHoldApplicationsAlertAsync(int days = 3)
        {
            return await _unitOfWork.ApplicationRepository
                .CountOnHoldOlderThanAsync(days);
        }

        public async Task<List<ApplicantExportDto>> ExportApplicantsAsync(
            ApplicationStatus? status,
            string? search)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Status", status);
            parameters.Add("@Search", search);

            var result = await _connection.QueryAsync<ApplicantExportDto>(
                "dbo.ExportApplicantsReport",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }
}
