using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.UserManagement
{
    public class Applicant : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public string City { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public MilitaryStatus MilitaryStatus { get; set; }
        public EducationDegree Degree { get; set; }
        public int GraduationYear { get; set; }
        public string Major { get; set; }
        public string CurrentJob { get; set; }
        public string CurrentEmployer { get; set; }
        public string CurrentSalary { get; set; }
        public string ExpectedSalary { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } 
        public string NoticePeriod { get; set; }
        public string ExtraCertificate { get; set; }
    }
}
