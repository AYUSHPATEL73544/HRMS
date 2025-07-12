using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Assest;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IAssetRepository
    {
        Task<MatTableResponse<AssetModel>> GetListAsync(MatDataTableRequest model);
        Task AddAsync(Asset entity);
        Task<bool> IsSerialNumberExistsAsync(string serialNumber);
        Asset Find(int id);
        Task<Asset> FindAsync(int id);
        void Update(Asset entity);
        Task<AssetModel> GetAsync(int id);

    }
}
