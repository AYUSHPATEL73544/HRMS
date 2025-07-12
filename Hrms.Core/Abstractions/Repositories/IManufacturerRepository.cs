using Hrms.Core.Models;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IManufacturerRepository
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int assetTypeId);

    }
}
