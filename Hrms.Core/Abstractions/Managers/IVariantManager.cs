using Hrms.Core.Models;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IVariantManager
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int ManufacturerId);
    }
}
