using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface ITitleRepository
    {
        Task<Title> GetByIdWithDepartmentsAsync(int id);
    }
}
