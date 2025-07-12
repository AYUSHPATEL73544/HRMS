using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Utilities;
using Hrms.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class EmployeeAttendanceRuleRepository : IEmployeeAttendanceRuleRepository
    {
        private readonly DataContext _dataContext;
        public EmployeeAttendanceRuleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(EmployeeAttendanceRule entity)
        {
            await _dataContext.AddAsync(entity);
        }
       
        public async Task<MatTableResponse<EmployeeAttendanceModel>> GetpagedListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from e in _dataContext.Employees
                           join ae in _dataContext.EmployeeAttendanceRules on e.Id equals ae.EmployeeId
                           join ar in _dataContext.AttendanceRules on ae.AttendanceRuleId equals ar.Id
                           join d in _dataContext.Departments on e.DepartmentId equals d.Id into deptGroup
                           from d in deptGroup.DefaultIfEmpty()
                           where ar.Status != Constants.RecordStatus.Deleted
                           && e.Status != Constants.RecordStatus.Inactive
                           && ae.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName,  "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%"))
                           select new EmployeeAttendanceModel
                           {
                               EmployeeId = ae.EmployeeId,
                               RuleId = ae.AttendanceRuleId,
                               EmployeeCode = e.Code,
                               Department = d.Name,
                               EmployeeName = e.FirstName + " "+ e.LastName,
                               RuleAssigned = ar.Title,
                               CreatedOn = ae.CreatedOn
                             
                               
                           };
            var response = new MatTableResponse<EmployeeAttendanceModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                       .OrderBy(sortExpression)
                       .Skip(recordsToSkip)
                       .Take(model.PageSize)
                       .ToListAsync()
            };

            return response;
        }

        public async Task<MatTableResponse<EmployeeAttendanceModel>> GetInActivepagedListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from e in _dataContext.Employees
                           join ae in _dataContext.EmployeeAttendanceRules on e.Id equals ae.EmployeeId
                           join ar in _dataContext.AttendanceRules on ae.AttendanceRuleId equals ar.Id
                           join d in _dataContext.Departments on e.DepartmentId equals d.Id into deptGroup
                           from d in deptGroup.DefaultIfEmpty()
                           where ar.Status != Constants.RecordStatus.Deleted 
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                            || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%"))
                           select new EmployeeAttendanceModel
                           {
                               EmployeeId = ae.EmployeeId,
                               RuleId = ae.AttendanceRuleId,
                               EmployeeCode = e.Code,
                               Department = d.Name,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               RuleAssigned = ar.Title,
                               Status = e.Status,
                               CreatedOn = ae.CreatedOn

                           };
            var response = new MatTableResponse<EmployeeAttendanceModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                       .OrderBy(sortExpression)
                       .Skip(recordsToSkip)
                       .Take(model.PageSize)
                       .ToListAsync()
            };

            return response;
        }

        public async Task<bool> IsExistAsync(int employeeId)
        {
           return await _dataContext.EmployeeAttendanceRules
            .AsNoTracking()
                .AnyAsync(x => x.EmployeeId == employeeId
                    && x.Status == Constants.RecordStatus.Active); 
        }

        public async Task<EmployeeAttendanceRule> GetByEmployeeIdAsync(int employeeId)
        {
            return await (from a in _dataContext.EmployeeAttendanceRules
                          where a.EmployeeId == employeeId && a.Status == Constants.RecordStatus.Active
                          select a).SingleOrDefaultAsync();

        }

        public async Task<List<EmployeeAttendanceRule>> GetByRuleIdAsync(int ruleId)
        {
            return await _dataContext.EmployeeAttendanceRules
                .Where(x => x.AttendanceRuleId == ruleId
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new EmployeeAttendanceRule
                {
                    Id = x.Id,
                    Status = x.Status
                }).ToListAsync();
        }

       

        public void Update(EmployeeAttendanceRule entity)
        {
            _dataContext.EmployeeAttendanceRules.Update(entity);
        }
    }
}
