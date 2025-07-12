using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class VariantRepository : IVariantRepository
    {
        private readonly DataContext _dataContext;
        public VariantRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync(int manufacturerId)
        {
            return await _dataContext.Variants
              .AsNoTracking()
              .Where(x => x.ManufacturerId == manufacturerId)
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
