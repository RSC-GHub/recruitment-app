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

                Dictionary<string, JsonElement> newValues = string.IsNullOrEmpty(log.NewValues)
                    ? new Dictionary<string, JsonElement>()
                    : JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(log.NewValues)!;

                Dictionary<string, JsonElement> oldValues = string.IsNullOrEmpty(log.OldValues)
                    ? new Dictionary<string, JsonElement>()
                    : JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(log.OldValues)!;

                // ==========================
                // Application status changed
                // ==========================
                if (log.TableName == "ApplicantApplication" &&
                    newValues.ContainsKey("ApplicationStatus") &&
                    oldValues.ContainsKey("ApplicationStatus") &&
                    newValues["ApplicationStatus"].GetInt32() != oldValues["ApplicationStatus"].GetInt32())
                {
                    int appId = JsonSerializer.Deserialize<Dictionary<string, int>>(log.KeyValues!)["Id"];
                    var app = await _unitOfWork.ApplicationRepository.GetByIdWithApplicantAsync(appId);

                    var oldStatus = (ApplicationStatus)oldValues["ApplicationStatus"].GetInt32();
                    var newStatus = (ApplicationStatus)newValues["ApplicationStatus"].GetInt32();

                    activities.Add(new RecentActivityDto
                    {
                        Icon = "bi-arrow-repeat text-primary",
                        Message = $"Application for {app.Applicant.FullName} changed status from {oldStatus} to {newStatus}",
                        Date = log.ChangedOn
                    });
                }

                // ==========================
                // Interview status changed
                // ==========================
                if (log.TableName == "Interview" &&
                    newValues.ContainsKey("InterviewStatus") &&
                    oldValues.ContainsKey("InterviewStatus") &&
                    newValues["InterviewStatus"].GetInt32() != oldValues["InterviewStatus"].GetInt32())
                {
                    int interviewId = JsonSerializer.Deserialize<Dictionary<string, int>>(log.KeyValues!)["Id"];
                    var interview = await _unitOfWork.InterviewRepository.GetByIdWithApplicantAsync(interviewId);

                    var oldStatus = (InterviewStatus)oldValues["InterviewStatus"].GetInt32();
                    var newStatus = (InterviewStatus)newValues["InterviewStatus"].GetInt32();

                    activities.Add(new RecentActivityDto
                    {
                        Icon = "bi-hourglass-split text-warning",
                        Message = $"Interview for {interview.Application.Applicant.FullName} scheduled on {interview.ScheduledDate:dd MMM yyyy} changed status from {oldStatus} to {newStatus}",
                        Date = log.ChangedOn
                    });
                }

                // ==========================
                // Interview result updated
                // ==========================
                if (log.TableName == "Interview" &&
                    newValues.ContainsKey("InterviewResult") &&
                    oldValues.ContainsKey("InterviewResult") &&
                    newValues["InterviewResult"].GetInt32() != oldValues["InterviewResult"].GetInt32())
                {
                    int interviewId = JsonSerializer.Deserialize<Dictionary<string, int>>(log.KeyValues!)["Id"];
                    var interview = await _unitOfWork.InterviewRepository.GetByIdWithApplicantAsync(interviewId);

                    var newResult = (InterviewResult)newValues["InterviewResult"].GetInt32();

                    activities.Add(new RecentActivityDto
                    {
                        Icon = "bi-check-circle-fill text-success",
                        Message = $"Interview for {interview.Application.Applicant.FullName} result updated: {newResult}",
                        Date = log.ChangedOn
                    });
                }
            }

            // ترتيب من الأحدث للأقدم وأخذ عدد الـ take المطلوب
            return activities.OrderByDescending(a => a.Date).Take(take).ToList();
        }



    }
}
