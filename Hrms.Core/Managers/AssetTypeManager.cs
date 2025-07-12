using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;


namespace Hrms.Core.Managers
{
    public class AssetTypeManager  : IAssetTypeManager
    {
        private readonly IAssetTypeRepository _assetTypeRepository;
        public AssetTypeManager(IAssetTypeRepository assetTypeRepository, IUserRepository userRepository)
        {
            _assetTypeRepository = assetTypeRepository;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _assetTypeRepository.GetSelectListItemsAsync();
        }

    }
}