using Recruitment.Application.DTOs.Audit;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.Audit;
using Recruitment.Domain.Enums;
using System.Text.Json;

namespace Recruitment.Application.Services.Audit
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int take = 10)
        {
            var logs = await _unitOfWork.AuditLogRepository.GetRecentAsync(take);
            var activities = new List<RecentActivityDto>();

            foreach (var log in logs)
            {
                if (log.ActionType != "Modified")
                    continue;

                var newValues = string.IsNullOrEmpty(log.NewValues)
                    ? new Dictionary<string, JsonElement>()
                    : JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(log.NewValues)!;

                var oldValues = string.IsNullOrEmpty(log.OldValues)
                    ? new Dictionary<string, JsonElement>()
                    : JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(log.OldValues)!;

                var keyValues = string.IsNullOrEmpty(log.KeyValues)
                    ? new Dictionary<string, int>()
                    : JsonSerializer.Deserialize<Dictionary<string, int>>(log.KeyValues)!;

                // ==========================
                // Application status changed
                // ==========================
                if (log.TableName == "ApplicantApplication" &&
                    newValues.TryGetValue("ApplicationStatus", out var newAppStatusElem) &&
                    oldValues.TryGetValue("ApplicationStatus", out var oldAppStatusElem))
                {
                    int newStatusInt = newAppStatusElem.GetInt32();
                    int oldStatusInt = oldAppStatusElem.GetInt32();

                    if (newStatusInt != oldStatusInt && keyValues.TryGetValue("Id", out int appId))
                    {
                        var app = await _unitOfWork.ApplicationRepository.GetByIdWithApplicantAsync(appId);

                        activities.Add(new RecentActivityDto
                        {
                            Icon = "bi-arrow-repeat text-primary",
                            Message = $"Application for {app?.Applicant?.FullName ?? "Unknown Applicant"} changed status from {(ApplicationStatus)oldStatusInt} to {(ApplicationStatus)newStatusInt}",
                            Date = log.ChangedOn
                        });
                    }
                }

                // ==========================
                // Interview status changed
                // ==========================
                if (log.TableName == "Interview" &&
                    newValues.TryGetValue("InterviewStatus", out var newInterviewStatusElem) &&
                    oldValues.TryGetValue("InterviewStatus", out var oldInterviewStatusElem))
                {
                    int newStatusInt = newInterviewStatusElem.GetInt32();
                    int oldStatusInt = oldInterviewStatusElem.GetInt32();

                    if (newStatusInt != oldStatusInt && keyValues.TryGetValue("Id", out int interviewId))
                    {
                        var interview = await _unitOfWork.InterviewRepository.GetByIdWithApplicantAsync(interviewId);

                        var scheduledDate = interview != null
                            ? interview.ScheduledDate.ToString("dd MMM yyyy")
                                : "N/A";


                        activities.Add(new RecentActivityDto
                        {
                            Icon = "bi-hourglass-split text-warning",
                            Message = $"Interview for {interview?.Application?.Applicant?.FullName ?? "Unknown Applicant"} scheduled on {scheduledDate} changed status from {(InterviewStatus)oldStatusInt} to {(InterviewStatus)newStatusInt}",
                            Date = log.ChangedOn
                        });
                    }
                }

                // ==========================
                // Interview result updated
                // ==========================
                if (log.TableName == "Interview" &&
                    newValues.TryGetValue("InterviewResult", out var newInterviewResultElem) &&
                    oldValues.TryGetValue("InterviewResult", out var oldInterviewResultElem))
                {
                    int newResultInt = newInterviewResultElem.GetInt32();

                    if (keyValues.TryGetValue("Id", out int interviewId))
                    {
                        var interview = await _unitOfWork.InterviewRepository.GetByIdWithApplicantAsync(interviewId);

                        activities.Add(new RecentActivityDto
                        {
                            Icon = "bi-check-circle-fill text-success",
                            Message = $"Interview for {interview?.Application?.Applicant?.FullName ?? "Unknown Applicant"} result updated: {(InterviewResult)newResultInt}",
                            Date = log.ChangedOn
                        });
                    }
                }
            }

            // Return most recent activities, safely taking only 'take' items
            return activities
                .OrderByDescending(a => a.Date)
                .Take(take)
                .ToList();
        }




    }
}
