using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly DataContext _dataContext;
        public ManufacturerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int assetTypeId)
        {
            return await _dataContext.Manufacturers
              .AsNoTracking()
              .Where(x => x.AssetTypeId == assetTypeId)
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
