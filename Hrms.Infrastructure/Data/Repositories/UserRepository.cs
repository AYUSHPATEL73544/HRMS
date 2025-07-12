using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        public UserRepository(DataContext dataContext,
            UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task CreateAsync(User entity, string password)
        {
            await _userManager.CreateAsync(entity, password);

            await _userManager.AddToRoleAsync(entity, Constants.UserType.Employee);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<SelectListItemModel>> GetListAsync()
        {
            return await (from u in _dataContext.Users
                          join r in _dataContext.UserRoles on u.Id equals r.UserId
                          where r.RoleId == 3 || r.RoleId == 6 || r.RoleId == 1
                          select new SelectListItemModel
                          {
                              Key = u.Id,
                              Value = u.FirstName + " " + u.LastName
                          }).OrderBy(x => x.Value)
                        .ToListAsync();

        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dataContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.UserName.Equals(email)
                    && (x.Status != Constants.RecordStatus.Deleted
                    || x.Status != Constants.RecordStatus.Inactive));
        }

        public async Task<int> SameEmailCountAsync(string email) //ToAsk : method name
        {
            return await _dataContext.Users
                        .Where(x => x.Email.StartsWith(email)).CountAsync();
        }

        public async Task<User> FindAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public void Update(User user)
        {
            _dataContext.Update(user);
        }
    }
}
