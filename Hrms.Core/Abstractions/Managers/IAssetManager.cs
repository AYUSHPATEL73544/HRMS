using Hrms.Core.Models;
using Hrms.Core.Models.Assest;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IAssetManager
    {
        Task<MatTableResponse<AssetModel>> GetListAsync(MatDataTableRequest model);
        Task<bool> IsSerialNumberExistsAsync(string serialNumber);
        Task AddAsync(AssetModel model);
        Task DeleteAsync(int id);
        Task<AssetModel> GetAsync(int id);
        Task UpdateAsync(AssetModel model, int userId);

    }
}
