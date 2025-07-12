using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Models;
using Hrms.Core.Abstractions.Managers;

namespace Hrms.Core.Managers
{
    public class CountryManager : ICountryManager
    {
        private readonly ICountryRepository _countryRepository;

        public CountryManager(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _countryRepository.GetSelectListItemsAsync();
        }
    }
}
