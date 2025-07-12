using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Assest;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class AssetManager : IAssetManager
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssetManager(IAssetRepository assetRepository, IUnitOfWork unitOfWork
          )
        {
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<MatTableResponse<AssetModel>> GetListAsync(MatDataTableRequest model)
        {
            return await _assetRepository.GetListAsync(model);
        }

        public async Task<bool> IsSerialNumberExistsAsync(string serialNumber)
        {
            return await _assetRepository.IsSerialNumberExistsAsync(serialNumber);
        }

        public async Task AddAsync(AssetModel model)
        {
            var asset = new Asset
            {
                Name = model.Name,
                PurchaseDate = model.PurchaseDate,
                IsInWarranty = model.IsInWarranty,
                WarrantyPeriod = model.WarrantyPeriod,
                SerialNumber = model.SerialNumber,
                VariantId = model.VariantId,
                AssetTypeId = model.AssetTypeId,
                ManufacturerId = model.ManufacturerId,
                VendorName = model.VendorName,
                Status = Constants.RecordStatus.Created
            };
            await _assetRepository.AddAsync(asset);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _assetRepository.Find(id);
            entity.Status = Constants.RecordStatus.Deleted;
            _assetRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AssetModel> GetAsync(int id)
        {
            return await _assetRepository.GetAsync(id);
        }
        public async Task UpdateAsync(AssetModel model, int userId)
        {
            var entity = await _assetRepository.FindAsync(model.Id);
            entity.AssetTypeId = model.AssetTypeId;
            entity.ManufacturerId = model.ManufacturerId;
            entity.VariantId = model.VariantId;
            entity.SerialNumber = model.SerialNumber;
            entity.WarrantyPeriod = model.WarrantyPeriod;
            entity.IsInWarranty = model.IsInWarranty;
            entity.Name = model.Name;
            entity.PurchaseDate = model.PurchaseDate;
            entity.VendorName = model.VendorName;
            _assetRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
