using Hrms.Core.Models;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IVariantRepository
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int manufacturerId);
    }
}
