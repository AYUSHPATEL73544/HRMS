using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _dataContext;

        public CityRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int stateId)
        {
            return await _dataContext.Cities
                .AsNoTracking()
                .Where(x => x.StateId == stateId
                    && x.Status != Constants.RecordStatus.Deleted)
                 .OrderBy(x => x.Name)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.Name
                })
                .Distinct()
                .OrderBy(x => x.Value)
                .ToListAsync();
        }
    }
}
