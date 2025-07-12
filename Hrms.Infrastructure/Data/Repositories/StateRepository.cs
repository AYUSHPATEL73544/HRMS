using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly DataContext _dataContext;
        public StateRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int countryId)
        {
            return await _dataContext.States
                .AsNoTracking()
                .Where(x => x.CountryId == countryId
                    && x.Status != Constants.RecordStatus.Deleted)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.Name
                })
                .OrderBy(x => x.Value)
                .ToListAsync();
        }
    }
}
