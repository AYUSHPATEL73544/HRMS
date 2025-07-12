using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class FamilyManager: IFamilyManager
    {
        private readonly IFamilyRepository _familyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public FamilyManager(IFamilyRepository familyRepository,
            IEmployeeRepository employeeRepository,
           IUnitOfWork unitOfWork)
        {
            _familyRepository = familyRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(FamilyModel model, int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);

            var family = new Family
            {
                EmployeeId = employee,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RelationshipId = model.RelationshipId,
                DateOfBirth = model.DateOfBirth,
                CreatedOn = Utility.GetDateTime(),
                Email = model.Email,
                Phone = model.Phone,
                Status = Constants.RecordStatus.Active,
                EffectiveFrom = Utility.GetDateTime()
            };

            await _familyRepository.AddAsync(family);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<FamilyModel>> GetAsync()
        {
            return await _familyRepository.GetAsync();
        }

        public async Task<FamilyModel>GetByIdAsync(int id)
        {
            return await _familyRepository.GetByIdAsync(id);
        }

        public async Task<MatTableResponse<FamilyModel>> GetByUserIdAsync(MatDataTableRequest model, int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);
            return await _familyRepository.GetPagedListAsync(model, employee);
        }

        public async Task<MatTableResponse<FamilyModel>> GetPagedListAsync(MatDataTableRequest model, int employeeId)
        {
            return await _familyRepository.GetPagedListAsync(model,employeeId);
        }

        public async Task UpdateAsync(FamilyModel model)
        {
            var family = await _familyRepository.FindAsync(model.Id);

            family.FirstName = model.FirstName;
            family.LastName = model.LastName;
            family.DateOfBirth = model.DateOfBirth;
            family.Email = model.Email;
            family.RelationshipId = model.RelationshipId;
            family.Phone = model.Phone;

            _familyRepository.Update(family);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _familyRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            _familyRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
