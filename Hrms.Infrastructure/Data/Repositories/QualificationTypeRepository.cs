using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class QualificationTypeRepository : IQualificationTypeRepository
    {
        private readonly DataContext _dataContext;
        public QualificationTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetSelectListItemsAsync()
        {
            return await _dataContext.QualificationTypes
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
