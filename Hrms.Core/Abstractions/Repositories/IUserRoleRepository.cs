using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Microsoft.AspNetCore.Identity;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddAsync(IdentityUserRole<int> entity);
        Task<List<SelectListItemModel>> GetRoleSelectListItemsAsync();
        Task<UserRoleModel> GetAsync(int id);
        Task<IdentityUserRole<int>> GetDetailAsync(int userId);
        void Delete(IdentityUserRole<int> entity);
    }
}

