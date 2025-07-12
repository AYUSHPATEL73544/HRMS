using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;


namespace Hrms.Core.Managers
{
    public class ManufacturerManager : IManufacturerManager
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerManager(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int assetTypeId)
        {
            return await _manufacturerRepository.GetSelectListItemsAsync(assetTypeId);
        }

    }
}
