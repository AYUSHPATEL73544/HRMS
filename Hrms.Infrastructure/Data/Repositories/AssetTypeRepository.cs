using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class AssetTypeRepository : IAssetTypeRepository
    {
        private readonly DataContext _dataContext;
        public AssetTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _dataContext.AssetTypes
                .AsNoTracking()
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
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
