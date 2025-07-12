using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Company;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _dataContext;
        public DepartmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Department entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<DepartmentModel> GetDetailAsync(int? id)
        {
            return await _dataContext.Departments
                .Where(x => x.Id == id)
                .Select(x => new DepartmentModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    Status = x.Status
                }).SingleOrDefaultAsync();
        }


        public async Task<MatTableResponse<DepartmentModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from d in _dataContext.Departments
                           where d.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(d.Name, "%" + model.FilterKey + "%"))
                           select new DepartmentModel
                           {
                               Id = d.Id,
                               Code = d.Code,
                               Name = d.Name,
                               Description = d.Description,
                               Status = d.Status,
                               
                           };

            var response = new MatTableResponse<DepartmentModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };

            foreach (var department in response.Items)
            {
                department.Peoples = await _dataContext.Employees.CountAsync(e => e.DepartmentId == department.Id
                && e.Status == Constants.RecordStatus.Active);
            }

            return response;

        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _dataContext.Departments
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.Name
                }).ToListAsync();
        }


        public async Task<Department> FindAsync(int id)
        {
            return await _dataContext.Departments.FindAsync(id);
        }

        public void Update(Department entity)
        {
            _dataContext.Departments.Update(entity);
        }

        public async Task<bool> IsExistsAsync(string departmentName)
        {
            return await (_dataContext.Departments
                .Where(x => x.Name == departmentName
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => x.Name)).AnyAsync();
        }
    }
}
