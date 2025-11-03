namespace Recruitment.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        // Audit fields
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
