using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Assest;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class AssetAllocationRepository : IAssetAllocationRepository
    {
        private readonly DataContext _dataContext;
        public AssetAllocationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddAsync(AssetAllocation entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<bool> IsAssignedAsync(int assetId)
        {
            return await _dataContext.AssetAllocation
                .AsNoTracking()
                .AnyAsync(x => x.AssetId.Equals(assetId)
                    && x.Status == Constants.RecordStatus.Active);
        }
        public async Task<bool> IsAssignedAsync(int employeeId , int assetId)
        {
            return await _dataContext.AssetAllocations
                .AsNoTracking()
                .AnyAsync(x => x.EmployeeId == employeeId && x.AssetId == assetId
                    && x.Status == Constants.RecordStatus.Active);
        }

        public async Task<AssetAllocation> GetAsync(int assetId)
        {
            return await _dataContext.AssetAllocation
                         .SingleOrDefaultAsync(x => x.AssetId == assetId
                         && x.Status == Constants.RecordStatus.Active);
        }

        public void Update(AssetAllocation entity)
        {
            _dataContext.AssetAllocations.Update(entity);
        }

        public async Task<List <AssetAllocationModel>> GetListAsync(int assetId)
        {
            return await (from asse in _dataContext.AssetAllocations
                          join em in _dataContext.Employees on asse.EmployeeId equals em.Id
                          where asse.AssetId == assetId
                          orderby asse.EffectiveFrom descending
                          select new AssetAllocationModel
                          {
                              Name = em.FirstName + " " + em.LastName,
                              EffectiveFrom = asse.EffectiveFrom,
                              EffectiveTo = asse.EffectiveTo,
                              Status = asse.Status,

                          }).ToListAsync();
        }
    }
}
