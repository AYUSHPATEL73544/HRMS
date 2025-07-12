using Hrms.Core.Entities;
using Hrms.Core.Models.Assest;

namespace Hrms.Core.Abstractions.Repositories
{
   public interface IAssetAllocationRepository
    {
        Task AddAsync(AssetAllocation entity);
        Task<bool> IsAssignedAsync(int assetId);
        Task<bool> IsAssignedAsync(int employeeId , int assetId);
        Task<AssetAllocation> GetAsync(int assetId);
        void Update(AssetAllocation entity);
        Task <List <AssetAllocationModel>> GetListAsync(int assetId);
    }
}
