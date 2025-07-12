using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IAssetTypeManager
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
