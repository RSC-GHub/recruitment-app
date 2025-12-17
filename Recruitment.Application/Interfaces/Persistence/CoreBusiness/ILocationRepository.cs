using Recruitment.Application.Common;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence.CoreBusiness
{
    public interface ILocationRepository
    {
        Task<PagedResult<Location>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? countryId = null);
    }
}
