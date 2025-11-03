namespace Recruitment.Domain.Entities.Aduit
{
    public class AuditLog : BaseEntity
    {
        public string TableName { get; set; } = string.Empty;
        public string? ActionType { get; set; }
        public string? KeyValues { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedOn { get; set; } = DateTime.UtcNow;
    }
}
