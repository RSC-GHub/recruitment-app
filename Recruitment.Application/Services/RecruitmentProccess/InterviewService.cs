using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Application.Services.RecruitmentProccess
{
    public class InterviewService : IInterviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<InterviewListDTO>> SearchAsync(
            string? search,
            InterviewStatus? status,
            InterviewResult? result,
            InterviewType? type,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize)
        {
            var paged = await _unitOfWork.InterviewRepository.SearchAsync(
                search, status, result, type, fromDate, toDate, page, pageSize);

            var dtoItems = paged.Items.Select(i => new InterviewListDTO
            {
                Id = i.Id,
                ApplicantName = i.Application.Applicant.FullName,
                VacancyTitle = i.Application.Vacancy.Title.Name,
                InterViewer = i.InterViewer,
                ScheduledDate = i.ScheduledDate,
                InterviewType = i.InterviewType,
                InterviewStatus = i.InterviewStatus,
                InterviewResult = i.InterviewResult,
                InterViewNote = i.InterViewNote
            }).ToList();

            return new PagedResult<InterviewListDTO>(dtoItems, paged.TotalCount, page, pageSize);
        }


        public async Task<PagedResult<InterviewListDTO>> GetPagedAsync(
                int page,
                int pageSize,
                string? search,
                InterviewStatus? status,
                InterviewResult? result,
                InterviewType? type,
                DateTime? fromDate,
                DateTime? toDate)

        {
            PagedResult<Interview> paged;

            if (!string.IsNullOrWhiteSpace(search))
            {
                paged = await _unitOfWork.InterviewRepository.SearchAsync(
                    search,
                    status,
                    result,
                    type,
                    fromDate,
                    toDate,
                    page,
                    pageSize
                );
            }
            else
            {
                paged = await _unitOfWork.InterviewRepository.GetPagedAsync(page, pageSize);
            }

            var dtoItems = paged.Items.Select(i => new InterviewListDTO
            {
                Id = i.Id,
                ApplicantName = i.Application?.Applicant?.FullName ?? "-",
                VacancyTitle = i.Application?.Vacancy?.Title?.Name ?? "-",
                InterViewer = i.InterViewer ?? "-",
                ScheduledDate = i.ScheduledDate,
                InterviewType = i.InterviewType,
                InterviewStatus = i.InterviewStatus,
                InterviewResult = i.InterviewResult,
                InterViewNote = i.InterViewNote,
            }).ToList();


            return new PagedResult<InterviewListDTO>(dtoItems, paged.TotalCount, page, pageSize);
        }

        public async Task<PagedResult<InterviewListDTO>> GetByApplicationIdAsync(int applicationId, int page, int pageSize)
        {
            var paged = await _unitOfWork.InterviewRepository.GetByApplicationIdAsync(applicationId, page, pageSize);

            var dtoItems = paged.Items.Select(i => new InterviewListDTO
            {
                Id = i.Id,
                ApplicantName = i.Application?.Applicant?.FullName ?? "-",
                VacancyTitle = i.Application?.Vacancy?.Title?.Name ?? "-",
                InterViewer = i.InterViewer ?? "-",
                ScheduledDate = i.ScheduledDate,
                InterviewType = i.InterviewType,
                InterviewStatus = i.InterviewStatus,
                InterviewResult = i.InterviewResult,
                InterViewNote = i.InterViewNote
            }).ToList();


            return new PagedResult<InterviewListDTO>(dtoItems, paged.TotalCount, page, pageSize);
        }

        public async Task<InterviewDetailDTO?> GetByIdAsync(int id)
        {
            var interview = await _unitOfWork.InterviewRepository.GetWithRelatedDataAsync(id);
            if (interview == null) return null;

            var vacancy = interview.Application?.Vacancy;
            var title = vacancy?.Title;

            return new InterviewDetailDTO
            {
                Id = interview.Id,
                ApplicationId = interview.ApplicationId,
                ApplicantId = interview.Application?.ApplicantId ?? 0,
                ApplicantName = interview.Application?.Applicant?.FullName ?? "-",
                PhoneNumber = interview.Application?.Applicant?.PhoneNumber ?? "-",
                ApplicantEmail = interview.Application?.Applicant?.Email ?? "-",
                VacancyTitle = title?.Name ?? "-",
                EmploymentType = vacancy?.EmploymentType ?? EmploymentType.FullTime,
                VacancyProjects = vacancy?.ProjectVacancies?
                                    .Where(pv => pv.Project != null)
                                    .Select(pv => pv.Project!.ProjectName)
                                    .ToList() ?? new List<string>(),

                InterViewer = interview.InterViewer,
                ScheduledDate = interview.ScheduledDate,
                InterviewType = interview.InterviewType,
                InterviewStatus = interview.InterviewStatus,
                InterviewResult = interview.InterviewResult,
                Feedback = interview.Feedback,
                DurationMinutes = interview.DurationMinutes,
                InterViewNote = interview.InterViewNote,

                CreatedBy = interview.CreatedBy,
                CreatedOn = interview.CreatedOn,
                ModifiedBy = interview.ModifiedBy,
                ModifiedOn = interview.ModifiedOn
            };
        }


        public async Task<bool> CreateAsync(InterviewCreateUpdateDTO dto)
        {
            var interview = new Interview
            {
                ApplicationId = dto.ApplicationId,
                InterViewer = dto.Interviewer,
                ScheduledDate = dto.ScheduledDate,
                InterviewType = dto.InterviewType,
                DurationMinutes = dto.DurationMinutes,
                InterviewStatus = dto.InterviewStatus ?? InterviewStatus.Scheduled,
                InterviewResult = dto.InterviewResult ?? InterviewResult.Pending,
                Feedback = dto.Feedback,
                InterViewNote = dto.InterViewNote,
            };

            await _unitOfWork.InterviewRepository.AddAsync(interview);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int id, InterviewCreateUpdateDTO dto)
        {
            var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(id);
            if (interview == null) return false;

            interview.InterViewer = dto.Interviewer;
            interview.ScheduledDate = dto.ScheduledDate;
            interview.InterviewType = dto.InterviewType;
            interview.DurationMinutes = dto.DurationMinutes;
            interview.Feedback = dto.Feedback;
            interview.InterViewNote = dto.InterViewNote;

            if (dto.InterviewStatus.HasValue)
                interview.InterviewStatus = dto.InterviewStatus.Value;

            if (dto.InterviewResult.HasValue)
                interview.InterviewResult = dto.InterviewResult.Value;

            if (dto.Feedback != null)
                interview.Feedback = dto.Feedback;

            _unitOfWork.InterviewRepository.Update(interview);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback, string? Note)
        {
            return await _unitOfWork.InterviewRepository.UpdateInterviewResultAsync(id, result, feedback, Note);
        }

        public async Task<bool> UpdateInterviewAsync(UpdateInterviewDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Calling the repository function to update
            bool result = await _unitOfWork.InterviewRepository.UpdateInterviewAsync(
                dto.Id,
                dto.InterViewer,
                dto.ScheduledDate,
                dto.InterviewType,
                dto.InterviewStatus,
                dto.DurationMinutes,
                dto.InterviewNote,
                dto.Feedback
            );

            return result;
        }
    }
}
