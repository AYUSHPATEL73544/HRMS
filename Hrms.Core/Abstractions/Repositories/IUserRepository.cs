using Hrms.Core.Entities;
using Hrms.Core.Models;
using Microsoft.AspNetCore.Identity;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User entity, string password);
        Task DeleteAsync(int userId);
        Task<List<SelectListItemModel>> GetListAsync();
        Task<bool> EmailExistsAsync(string email);
        Task<int> SameEmailCountAsync(string email);
        Task<User> FindAsync(int id);
        void Update(User user);
    }
}
