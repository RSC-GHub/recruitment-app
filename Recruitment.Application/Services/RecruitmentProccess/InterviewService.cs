using Recruitment.Application.Common;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Application.Interfaces.Persistence;
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

        public async Task<PagedResult<InterviewListDTO>> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            PagedResult<Interview> paged;

            if (!string.IsNullOrWhiteSpace(search))
            {
                paged = await _unitOfWork.InterviewRepository.SearchAsync(search, null, null, page, pageSize);
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

        public async Task<PagedResult<InterviewListDTO>> SearchAsync(
            string? interviewer,
            InterviewStatus? status,
            InterviewResult? result,
            int page,
            int pageSize)
        {
            var paged = await _unitOfWork.InterviewRepository.SearchAsync(interviewer, status, result, page, pageSize);

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

        public async Task<InterviewDetailDTO?> GetByIdAsync(int id)
        {
            var interview = await _unitOfWork.InterviewRepository.GetWithApplicationAsync(id);
            if (interview == null) return null;

            return new InterviewDetailDTO
            {
                Id = interview.Id,
                ApplicationId = interview.ApplicationId,
                ApplicantName = interview.Application.Applicant.FullName,
                ApplicantEmail = interview.Application.Applicant.Email,
                VacancyTitle = interview.Application.Vacancy.Title.Name,
                InterViewer = interview.InterViewer,
                ScheduledDate = interview.ScheduledDate,
                InterviewType = interview.InterviewType,
                InterviewStatus = interview.InterviewStatus,
                InterviewResult = interview.InterviewResult,
                Feedback = interview.Feedback,
                DurationMinutes = interview.DurationMinutes,
                InterViewNote = interview.InterViewNote,
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

        public async Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback)
        {
            return await _unitOfWork.InterviewRepository.UpdateInterviewResultAsync(id, result, feedback);
        }

        public async Task<bool> UpdateInterviewStatusAsync(int id, InterviewStatus status)
        {
            return await _unitOfWork.InterviewRepository.UpdateInterviewStatusAsync(id, status);
        }
    }

}
