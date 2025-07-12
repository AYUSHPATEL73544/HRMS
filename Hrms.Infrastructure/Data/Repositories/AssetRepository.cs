using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Assest;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly DataContext _dataContext;
        public AssetRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<MatTableResponse<AssetModel>> GetListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from asset in _dataContext.Assets
                           where asset.Status == Constants.RecordStatus.Created && (model.FilterKey == null
                           || EF.Functions.Like(asset.Name, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(asset.SerialNumber, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(asset.VendorName , "%" + model.FilterKey + "%"))

                           select new AssetModel
                           {
                               Id = asset.Id,
                               Name = asset.Name,
                               WarrantyPeriod = asset.WarrantyPeriod,
                               IsInWarranty = asset.IsInWarranty,
                               PurchaseDate = asset.PurchaseDate,
                               SerialNumber = asset.SerialNumber,
                               VendorName = asset.VendorName,

                           };

            var response = new MatTableResponse<AssetModel>
            {

                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };
            return response;
        }
        public async Task AddAsync(Asset entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<bool> IsSerialNumberExistsAsync(string serialNumber)
        {
            return await _dataContext.Assets
                .AsNoTracking()
                .AnyAsync(x => x.SerialNumber.Equals(serialNumber)
                    && x.Status == Constants.RecordStatus.Created);
        }


        public Asset Find(int id)
        {
            return _dataContext.Assets.Find(id);
        }
        public async Task<Asset> FindAsync(int id)
        {
            return await _dataContext.Assets.FindAsync(id);
        }
        public void Update(Asset entity)
        {
            _dataContext.Assets.Update(entity);
        }
        public async Task<AssetModel> GetAsync(int id)
        {

            return await (from asse in _dataContext.Assets
                          join ma in _dataContext.Manufacturers on asse.ManufacturerId equals ma.Id
                          where asse.Id == id
                          select new AssetModel
                          {
                              Id = asse.Id,
                              AssetTypeId = asse.AssetTypeId,
                              Name = asse.Name,
                              ManufacturerId = asse.ManufacturerId,
                              VariantId = asse.VariantId,
                              SerialNumber = asse.SerialNumber,
                              PurchaseDate = asse.PurchaseDate,
                              VendorName = asse.VendorName,
                              IsInWarranty = asse.IsInWarranty,
                              WarrantyPeriod = asse.WarrantyPeriod,
                              ManufacturerName = ma.Name,


                          }).SingleOrDefaultAsync();
         
        }

    }
}

