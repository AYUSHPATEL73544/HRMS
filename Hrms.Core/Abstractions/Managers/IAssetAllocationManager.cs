using Hrms.Core.Models.Assest;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IAssetAllocationManager
    {
        Task AddAsync(AssetAllocationModel model , int userId);
        Task<bool> IsAssignedAsync(int employeeId , int assetId);
        Task <List<AssetAllocationModel>> GetListAsync(int assetId);
    }
}
