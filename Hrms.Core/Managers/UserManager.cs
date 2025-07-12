using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _identityUserManager;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(UserManager<User> identityUserManager,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _identityUserManager = identityUserManager;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _identityUserManager.CheckPasswordAsync(user, password);
        }

        public async Task<List<string>> GetRolesAsync(User user)
        {
            return (await _identityUserManager.GetRolesAsync(user)).ToList();
        }

        public async Task<List<SelectListItemModel>> GetListAsync()
        {
            return await _userRepository.GetListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _identityUserManager.Users
               .SingleOrDefaultAsync(x => x.Email == email
                   && x.Status != Constants.RecordStatus.Inactive);
        }

        public async Task ChangePasswordAsync(User user, string currentPassword, string password)
        {
            var result = await _identityUserManager.ChangePasswordAsync(user, currentPassword, password);
            if(!result.Succeeded)
            {
                throw new InvalidDataException(result.Errors.Select(x => x.Description).FirstOrDefault());
            }
            user.UpdateOn = Utility.GetDateTime();
            user.UpdatedById = user.Id;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ResetPasswordAsync(User user ,string password, int userId)
        {
            var resetToken = await _identityUserManager.GeneratePasswordResetTokenAsync(user);

            var result = await _identityUserManager.ResetPasswordAsync(user,resetToken ,password);
            if (!result.Succeeded)
            {
                throw new InvalidDataException(result.Errors.Select(x => x.Description).FirstOrDefault());
            }
            user.UpdateOn = Utility.GetDateTime();
            user.UpdatedById = userId;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User> FindAsync(int id)
        {
            return await _userRepository.FindAsync(id);
        }
    }
}
