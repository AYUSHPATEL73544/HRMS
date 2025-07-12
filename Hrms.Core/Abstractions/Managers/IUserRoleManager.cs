using Hrms.Core.Models;
using Hrms.Core.Models.Employee;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IUserRoleManager
    {
        Task AddAsync(UserRoleModel model);
        Task<List<SelectListItemModel>> GetRoleSelectListItemsAsync();
        Task<UserRoleModel> GetAsync(int id);
    }
}
