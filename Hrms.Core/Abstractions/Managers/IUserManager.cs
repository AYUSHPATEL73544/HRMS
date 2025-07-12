using Hrms.Core.Entities;
using Hrms.Core.Models;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IUserManager
    {
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<List<string>> GetRolesAsync(User user);
        Task<User> GetByEmailAsync(string email);
        Task<List<SelectListItemModel>> GetListAsync();
        Task ChangePasswordAsync(User user, string currentPassword, string password);
        Task<User> FindAsync(int id);
        Task ResetPasswordAsync(User user, string password, int userId);
    }
}
