using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Microsoft.AspNetCore.Identity;

namespace Hrms.Core.Managers
{
    public class UserRoleManager : IUserRoleManager
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserRoleManager(IUserRoleRepository userRoleRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _userRoleRepository = userRoleRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(UserRoleModel model)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userId = await _employeeRepository.GetUserIdAsync(model.UserId);

                var entity = await _userRoleRepository.GetDetailAsync(userId);
                _userRoleRepository.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                var userRole = new IdentityUserRole<int>
                {
                    UserId = userId,
                    RoleId = model.RoleId
                };
                await _userRoleRepository.AddAsync(userRole);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<UserRoleModel> GetAsync(int id)
        {
            var userId = await _employeeRepository.GetUserIdAsync(id);
            return await _userRoleRepository.GetAsync(userId);
        }

        public async Task<List<SelectListItemModel>> GetRoleSelectListItemsAsync()
        {
            return await _userRoleRepository.GetRoleSelectListItemsAsync();
        }
    }
}

