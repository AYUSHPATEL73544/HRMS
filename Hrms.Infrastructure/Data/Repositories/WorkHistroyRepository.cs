using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class WorkHistroyRepository: IWorkHistroyRepository
    {
        private readonly DataContext _dataContext;
        public WorkHistroyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(WorkHistory entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<List<WorkHistoryModel>> GetAsync(int employeeId)
        {
            return await (from wh in _dataContext.WorkHistories
                          join d in _dataContext.Departments on wh.DepartmentId equals d.Id
                          join ds in _dataContext.Designations on wh.DesignationId equals ds.Id
                          where wh.EmployeeId == employeeId 
                               && wh.Status != Constants.RecordStatus.Deleted
                          select new WorkHistoryModel
                          {
                              Id = wh.Id,
                              EmployeeId = wh.EmployeeId,
                              DepartmentId = wh.DepartmentId,
                              DepartmentName = d.Name,
                              DesignationId = wh.DesignationId,
                              DesignationName = ds.Name,
                              From = wh.From,
                              To = wh.To,
                              Status = wh.Status
                          }).ToListAsync();
        }

        public async Task<WorkHistoryModel> GetByIdAsync(int id)
        {
            return await _dataContext.WorkHistories
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new WorkHistoryModel
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    DepartmentId = x.DepartmentId,
                    DesignationId = x.DesignationId,
                    From = x.From,
                    To = x.To,
                    Status = x.Status
                }).SingleOrDefaultAsync();
        }

        public async Task<WorkHistory> GetByPreviousWorkHistoryAsync(int? departmentId, int? designationId, int employeeId)
        {
            return await _dataContext.WorkHistories
                .Where(x => x.DepartmentId == departmentId
                && x.DesignationId == designationId
                && x.EmployeeId == employeeId
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new WorkHistory
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    DepartmentId = x.DepartmentId,
                    DesignationId = x.DesignationId,
                    From = x.From,
                    To = x.To,
                    Status = x.Status
                }).SingleOrDefaultAsync();
        }

        public async Task<WorkHistory> FindAsync(int id)
        {
            return await _dataContext.WorkHistories.FindAsync(id);
        }

        public void Update(WorkHistory entity)
        {
            _dataContext.WorkHistories.Update(entity);
        }
    }
}
