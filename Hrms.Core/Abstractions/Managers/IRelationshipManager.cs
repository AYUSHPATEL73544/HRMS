using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IRelationshipManager
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
