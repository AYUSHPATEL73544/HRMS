using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IRelationshipRepository
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
