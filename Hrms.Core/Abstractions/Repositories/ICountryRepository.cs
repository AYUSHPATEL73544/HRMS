using Hrms.Core.Models;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
