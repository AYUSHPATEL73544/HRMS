using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;


namespace Hrms.Core.Managers
{
    public class VariantManager : IVariantManager
    {
        private readonly IVariantRepository _variantRepository;
        public VariantManager(IVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync( int ManufacturerId)
        {
            return await _variantRepository.GetSelectListItemsAsync(ManufacturerId);
        }
    }
}
