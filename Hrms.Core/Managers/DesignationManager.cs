using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Entities;
using Hrms.Core.Models.Company;
using Hrms.Core.Utilities;
using Hrms.Core.Models;

namespace Hrms.Core.Managers
{
    public class DesignationManager : IDesignationManager
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DesignationManager(IDesignationRepository designationRepository,
           IUnitOfWork unitOfWork)
        {
            _designationRepository = designationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(DesignationModel model)
        {
            var designations = new Designation
            {
                Name = model.Name,
                Description = model.Description,
                Status = Constants.RecordStatus.Active
            };
            await _designationRepository.AddAsync(designations);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<DesignationModel> GetDetailAsync(int id)
        {
            return await _designationRepository.GetDetailAsync(id);
        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _designationRepository.GetSelectListItemAsync();
        }

        public async Task UpdateAsync(DesignationModel model)
        {
            var designation = await _designationRepository.FindAsync(model.Id);

            designation.Name = model.Name;
            designation.Description = model.Description;

            _designationRepository.Update(designation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<MatTableResponse<DesignationModel>> GetPageListAsync(MatDataTableRequest model)
        {
            return await _designationRepository.GetPageListAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _designationRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            _designationRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsDesignationExistAsync(string designation)
        {
            return await _designationRepository.IsDesignationExistAsync(designation);
        }
    }
}
