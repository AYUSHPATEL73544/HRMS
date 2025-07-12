using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hrms.Core.Models;
using Hrms.Core.Abstractions.Managers;

namespace Hrms.Core.Managers
{
    public class StateManager : IStateManager
    {
        private readonly IStateRepository _stateRepository;
        public StateManager(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int countryId)
        {
            return await _stateRepository.GetSelectListItemsAsync(countryId);
        }
    }
}
