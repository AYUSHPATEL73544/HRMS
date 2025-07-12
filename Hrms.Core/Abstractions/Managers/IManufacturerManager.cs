using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IManufacturerManager
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int assetTypeId);
    }
}
