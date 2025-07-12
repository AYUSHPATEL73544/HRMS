using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IQualificationTypeManager
    {
        Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync();
    }
}
