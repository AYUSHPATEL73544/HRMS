using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Managers;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DataContext _dataContext;
        public UserRoleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(IdentityUserRole<int> entity)
        {
            await _dataContext.UserRoles.AddAsync(entity);
        }

        public async Task<UserRoleModel> GetAsync(int id)
        {
            var response = await (from ur in _dataContext.UserRoles
                                  join r in _dataContext.Roles on ur.RoleId equals r.Id
                                  where ur.UserId == id
                                  select new UserRoleModel
                                  {
                                      UserId = ur.UserId,
                                      RoleId = ur.RoleId,
                                      RoleName = r.DisplayName,
                                  }).SingleOrDefaultAsync();


            return response;
        }

        public async Task<IdentityUserRole<int>> GetDetailAsync(int userId)
        {
            return await _dataContext.UserRoles
                         .Where(x => x.UserId == userId)
                         .SingleAsync();
        }

        public async Task<List<SelectListItemModel>> GetRoleSelectListItemsAsync()
        {
            return await _dataContext.Roles
             .Select(x => new SelectListItemModel
             {
                 Key = x.Id,
                 Value = x.DisplayName
             }).ToListAsync();
        }

        public void Delete(IdentityUserRole<int> entity)
        {
            _dataContext.UserRoles.Remove(entity);
        }
    }
}

