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

        public async Task<PagedResult<InterviewListDTO>> SearchAsync(
            string? search,
            InterviewStatus? status,
            InterviewResult? result,
            InterviewType? type,
            InterviewCategory? category,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize)
        {
            var paged = await _unitOfWork.InterviewRepository.SearchAsync(
                search, status, result, type, category, fromDate, toDate, page, pageSize);

            var dtoItems = paged.Items.Select(i => new InterviewListDTO
            {
                Id = i.Id,
                ApplicantName = i.Application.Applicant.FullName,
                VacancyTitle = i.Application.Vacancy.Title!.Name,
                InterviewerName = i.Interviewer.Name,
                ScheduledDate = i.ScheduledDate,
                InterviewType = i.InterviewType,
                InterviewCategory = i.InterviewCategory,
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
                InterviewCategory? category,
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
                    category,
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
                InterviewerName = i.Interviewer.Name,
                ScheduledDate = i.ScheduledDate,
                InterviewType = i.InterviewType,
                InterviewCategory = i.InterviewCategory,
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
                InterviewerName = i.Interviewer.Name,
                ScheduledDate = i.ScheduledDate,
                InterviewType = i.InterviewType,
                InterviewCategory = i.InterviewCategory,
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

                InterviewerId = interview.InterviewerId,
                InterviewerName = interview.Interviewer.Name,
                ScheduledDate = interview.ScheduledDate,
                InterviewType = interview.InterviewType,
                InterviewCategory = interview.InterviewCategory,
                InterviewStatus = interview.InterviewStatus,
                InterviewResult = interview.InterviewResult,
                Feedback = interview.Feedback,
                DurationMinutes = interview.DurationMinutes,
                InterViewNote = interview.InterViewNote,

                RejectionReasonIds = interview.RejectionReasons
                                    .Select(r => r.RejectionReasonId)
                                    .ToList(),

                RejectionReasonTexts = interview.RejectionReasons
                                    .Select(r => r.RejectionReason.Reason)
                                    .ToList(),

                CreatedBy = interview.CreatedBy,
                CreatedOn = interview.CreatedOn,
                ModifiedBy = interview.ModifiedBy,
                ModifiedOn = interview.ModifiedOn
            };
        }


        public async Task<bool> CreateAsync(InterviewCreateUpdateDTO dto)
        {
            if (dto.ScheduledDate < DateTime.Now)
                throw new ArgumentException("Scheduled date cannot be in the past.");

            var newStart = dto.ScheduledDate;
            var newEnd = dto.ScheduledDate.AddMinutes(dto.DurationMinutes);

            var interviewerConflict =
                await _unitOfWork.InterviewRepository
                    .HasOverlappingInterviewAsync(dto.InterviewerId, newStart, newEnd);

            if (interviewerConflict)
                throw new InvalidOperationException(
                    "Interviewer already has another interview during this time slot.");
            var applicationConflict =
                await _unitOfWork.InterviewRepository
                    .HasOverlappingInterviewAsync(
                        dto.ApplicationId, newStart, newEnd);

            if (applicationConflict)
                throw new InvalidOperationException(
                    "This application already has another interview during this time slot.");

            var interview = new Interview
            {
                ApplicationId = dto.ApplicationId,
                InterviewerId = dto.InterviewerId,
                ScheduledDate = dto.ScheduledDate,
                DurationMinutes = dto.DurationMinutes,
                InterviewType = dto.InterviewType,
                InterviewCategory = dto.InterviewCategory,
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

            interview.InterviewerId = dto.InterviewerId;
            interview.ScheduledDate = dto.ScheduledDate;
            interview.InterviewType = dto.InterviewType;
            interview.InterviewCategory = dto.InterviewCategory;
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

        public async Task<int?> UpdateInterviewResultAsync(
            int interviewId,
            InterviewResult result,
            string? feedback,
            string? note,
            List<int>? rejectionReasonIds = null)
        {
            var interview = await _unitOfWork.InterviewRepository
                .GetWithRejectionReasonsAsync(interviewId);

            if (interview == null)
                return null;

            if (interview.Application == null)
                throw new InvalidOperationException("Interview application not found");

            interview.InterviewResult = result;
            interview.Feedback = feedback;
            interview.InterViewNote = note;
            interview.InterviewStatus = InterviewStatus.Completed;

            switch (result)
            {
                case InterviewResult.Accepted:
                    interview.Application.ApplicationStatus = ApplicationStatus.AcceptedInterview;
                    interview.RejectionReasons.Clear();
                    break;

                case InterviewResult.SecondChoice:
                    interview.Application.ApplicationStatus = ApplicationStatus.Pending;
                    interview.RejectionReasons.Clear();
                    break;

                case InterviewResult.Rejected:
                    interview.Application.ApplicationStatus = ApplicationStatus.Rejected;
                    interview.RejectionReasons.Clear();

                    if (rejectionReasonIds != null && rejectionReasonIds.Any())
                    {
                        foreach (var reasonId in rejectionReasonIds)
                        {
                            interview.RejectionReasons.Add(new InterviewRejectionReason
                            {
                                InterviewId = interview.Id,
                                RejectionReasonId = reasonId
                            });
                        }
                    }
                    break;

                default:
                    interview.RejectionReasons.Clear();
                    break;
            }

            _unitOfWork.InterviewRepository.Update(interview);
            await _unitOfWork.CompleteAsync();
            return interview.ApplicationId;
        }

        public async Task<bool> UpdateInterviewAsync(UpdateInterviewDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Calling the repository function to update
            bool result = await _unitOfWork.InterviewRepository.UpdateInterviewAsync(
                dto.Id,
                dto.InterviewerId,
                dto.InterviewStatus,
                dto.InterviewType,
                dto.DurationMinutes,
                dto.InterviewNote
            );

            return result;
        }

        public Task<int> CountTodaysInterviewsAsync()
        {
            var count = _unitOfWork.InterviewRepository.CountTodaysInterviewsAsync();
            return count;
        }

        public async Task<int> GetPendingInterviewResultsAlertAsync()
        {
            return await _unitOfWork.InterviewRepository
                .CountPendingInterviewResultsAsync();
        }

        public async Task<List<InterviewCalendarDto>> GetInterviewsForCalendarAsync(int? month = null, int? year = null)
        {
            var interviews = await _unitOfWork.InterviewRepository.GetAllForCalendarAsync(month, year);

            var dtos = interviews.Select(i => new InterviewCalendarDto
            {
                Id = i.Id,
                ScheduledDate = i.ScheduledDate,
                ApplicantName = i.Application?.Applicant?.FullName ?? "N/A",
                VacancyTitle = i.Application?.Vacancy?.Title?.Name ?? "N/A"
            }).ToList();

            return dtos;
        }
    }
}
