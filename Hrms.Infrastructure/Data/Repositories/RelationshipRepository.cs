using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class RelationshipRepository : IRelationshipRepository
    {
        private readonly DataContext _dataContext;
        public RelationshipRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _dataContext.Relationships
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
