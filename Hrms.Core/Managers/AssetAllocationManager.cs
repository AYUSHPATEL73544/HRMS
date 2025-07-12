using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Assest;
using Hrms.Core.Utilities;


namespace Hrms.Core.Managers
{
    public class AssetAllocationManager : IAssetAllocationManager
    {
        private readonly IAssetAllocationRepository _assetAllocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssetAllocationManager(IAssetAllocationRepository assetAllocationRepository, IUnitOfWork unitOfWork)
        {
            _assetAllocationRepository = assetAllocationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(AssetAllocationModel model, int userId)
        {
            if (await _assetAllocationRepository.IsAssignedAsync(model.AssetId))
            {
                var entity = await _assetAllocationRepository.GetAsync(model.AssetId);
                entity.Status = Constants.RecordStatus.Inactive;
                entity.EffectiveTo = Utility.GetDateTime();
                entity.UpdatedById = userId;
                _assetAllocationRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            var assetAllocation = new AssetAllocation
            {

                AssetId = model.AssetId,
                EmployeeId = model.EmployeeId,
                EffectiveFrom = model.EffectiveFrom,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
                CreatedById = userId,

            };
            await _assetAllocationRepository.AddAsync(assetAllocation);
            await _unitOfWork.SaveChangesAsync();

        }
        public async Task<bool> IsAssignedAsync(int employeeId , int assetId)
        {
            return await _assetAllocationRepository.IsAssignedAsync(employeeId , assetId);
        }

        public async Task<List<AssetAllocationModel>> GetListAsync(int assetId)
        {
            return await _assetAllocationRepository.GetListAsync(assetId);
        }
    }
}
