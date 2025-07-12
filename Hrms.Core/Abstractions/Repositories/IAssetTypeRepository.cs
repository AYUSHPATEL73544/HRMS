using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IAssetTypeRepository
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
