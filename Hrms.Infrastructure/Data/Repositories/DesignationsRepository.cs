using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Company;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Utilities;
using Hrms.Core.Models;
using System.Security.Cryptography.X509Certificates;
using Hrms.Core.Models.Employee;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class DesignationsRepository : IDesignationRepository
    {
        private readonly DataContext _dataContext;
        public DesignationsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Designation entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<DesignationModel> GetDetailAsync(int? id)
        {
            return await _dataContext.Designations
                .Where(x => x.Id == id)
                .Select(x => new DesignationModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status
                }).SingleOrDefaultAsync();
        }


        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _dataContext.Designations
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.Name
                }).ToListAsync();
        }

        public async Task<Designation> FindAsync(int id)
        {
            return await _dataContext.Designations.FindAsync(id);
        }

        public async Task<MatTableResponse<DesignationModel>> GetPageListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from d in _dataContext.Designations
                           where d.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(d.Name, "%" + model.FilterKey + "%"))
                           select new DesignationModel
                           {
                               Id = d.Id,
                               Name = d.Name,
                               Status = d.Status,
                               Description = d.Description
                              
                           };

            var response = new MatTableResponse<DesignationModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };

            foreach (var designation in response.Items)
            {
                designation.Peoples = await _dataContext.Employees.CountAsync(e => e.DesignationId == designation.Id
                                                                               && e.Status == Constants.RecordStatus.Active);
            }

            return response;
        }

        public void Update(Designation entity)
        {
            _dataContext.Designations.Update(entity);
        }

        public async Task<bool> IsDesignationExistAsync(string designation)
        {
            return await (_dataContext.Designations
                .Where(x => x.Name == designation
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => x.Name)).AnyAsync();
        }
    }
}
