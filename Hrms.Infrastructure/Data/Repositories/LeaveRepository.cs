using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly DataContext _dataContext;
        public LeaveRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Leave entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<Leave> GetByRuleIdAsync(int employeeId, int ruleId)
        {
            return await _dataContext.Leaves
              .Where(x => x.RuleId == ruleId
              && x.EmployeeId == employeeId
              && x.Status == Constants.RecordStatus.Active)
              .Select(x => new Leave
              {
                  Id = x.Id,
                  Total = x.Total,
                  Credited = x.Credited,
                  EmployeeId = x.EmployeeId,
                  RuleId = x.RuleId,
                  Applied = x.Applied,
                  Available = x.Available,
                  Status = x.Status,
              }).SingleOrDefaultAsync();
        }

        public async Task<List<Leave>> GetByRuleIdAsync(int ruleId)
        {
            return await _dataContext.Leaves
               .Where(x => x.RuleId == ruleId
               && x.Status != Constants.RecordStatus.Deleted)
               .Select(x => new Leave
               {
                   Id = x.Id,
                   Total = x.Total,
                   Credited = x.Credited,
                   EmployeeId = x.EmployeeId,
                   RuleId = x.RuleId,
                   Applied = x.Applied,
                   Available = x.Available,
                   Status = x.Status,
               }).ToListAsync();
        }

        public async Task<LeaveModel> GetAsync(int id)
        {

            return await _dataContext.Leaves
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new LeaveModel
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    RuleId = x.RuleId,
                    Total = x.Total,
                    Credited = x.Credited,
                    Available = x.Available,
                    Applied = x.Applied
                }).SingleAsync();
        }

        public async Task<List<LeaveModel>> GetListAsync()
        {
            var leaves = await (from l in _dataContext.Leaves
                                where (l.Status != Constants.RecordStatus.Deleted)
                                select new LeaveModel
                                {
                                    Id = l.Id,
                                    EmployeeId = l.EmployeeId,
                                    RuleId = l.RuleId,
                                    Total = l.Total,
                                    Credited = l.Credited,
                                    Applied = l.Applied,
                                    Available = l.Available
                                })
                              .ToListAsync();
            return leaves;

        }

        public async Task<MatTableResponse<LeaveModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from l in _dataContext.Leaves
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           join lr in _dataContext.LeaveRules on l.RuleId equals lr.Id
                           join d in _dataContext.Departments on e.DepartmentId equals d.Id into deptGroup
                           from d in deptGroup.DefaultIfEmpty()
                           where  l.Status == Constants.RecordStatus.Active 
                           && e.Status != Constants.RecordStatus.Inactive
                           && (model.FilterKey == null || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%") 
                           || EF.Functions.Like(e.FirstName +" " + e.LastName, "%" + model.FilterKey + "%") 
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(lr.Title, "%" + model.FilterKey + "%"))
                           select new LeaveModel
                           {
                               EmployeeId = l.EmployeeId,
                               RuleId = l.RuleId,
                               Total = lr.MaxAllowedInYear,
                               Credited = l.Credited,
                               Applied = l.Applied,
                               Available = l.Available,
                               EmployeeCode = e.Code,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               LeaveRule = lr.Title,
                               DepartmentId = e.DepartmentId,
                               Department = d.Name,
                               Status = lr.Status,
                               LeaveRuleStatus = lr.Status,
                               CreatedOn = l.CreatedOn
                           };

            var response = new MatTableResponse<LeaveModel>
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

        public async Task<MatTableResponse<LeaveModel>> GetAssignRuleListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from l in _dataContext.Leaves
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           where e.Status == Constants.RecordStatus.Active
                                && l.Status == Constants.RecordStatus.Active
                                && (model.FilterKey == null || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code,"%" + model.FilterKey + "%"))
                           
                           group new { l , e } by l.EmployeeId into employeeGroup
                   select new LeaveModel
                   {
                       EmployeeId = employeeGroup.Select(x => x.l.EmployeeId).FirstOrDefault(),
                       EmployeeName = employeeGroup.Select(x => x.e.FirstName + " " + x.e.LastName).FirstOrDefault(),
                       EmployeeCode = employeeGroup.Select(x => x.e.Code).FirstOrDefault(),
                       DepartmentId =  employeeGroup.Select(x => x.e.DepartmentId).FirstOrDefault(),
                       Status = employeeGroup.Select(x => x.e.Status).FirstOrDefault(),
                       CreatedOn = employeeGroup.Select(x => x.l.CreatedOn).FirstOrDefault(),
                   };

            var response = new MatTableResponse<LeaveModel>
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
       
        public async Task<MatTableResponse<LeaveModel>> GetInactiveAssignListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from l in _dataContext.Leaves
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           where (e.Status == Constants.RecordStatus.Active 
                           || e.Status == Constants.RecordStatus.Inactive)
                           && l.Status == Constants.RecordStatus.Active
                           && (model.FilterKey == null || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%"))

                           group new { l, e } by l.EmployeeId into employeeGroup
                           select new LeaveModel
                           {
                               EmployeeId = employeeGroup.Select(x => x.l.EmployeeId).FirstOrDefault(),
                               EmployeeName = employeeGroup.Select(x => x.e.FirstName + " " + x.e.LastName).FirstOrDefault(),
                               EmployeeCode = employeeGroup.Select(x => x.e.Code).FirstOrDefault(),
                               DepartmentId = employeeGroup.Select(x => x.e.DepartmentId).FirstOrDefault(),
                               Status = employeeGroup.Select(x => x.e.Status).FirstOrDefault(),
                               CreatedOn = employeeGroup.Select(x => x.l.CreatedOn).FirstOrDefault(),
                               // Department = employeeGroup.Select(x => x.d.Name != null ? x.d.Name : "" ).FirstOrDefault()

                           };

            var response = new MatTableResponse<LeaveModel>
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

        public async Task<List<LeaveModel>> GetListAsync(int employeeId)
        {
            var currentDate = Utility.GetDateTime().Date;

            var leaves = await (from l in _dataContext.Leaves
                                join lr in _dataContext.LeaveRules on l.RuleId equals lr.Id
                                where (l.EmployeeId == employeeId
                                && lr.Status == Constants.RecordStatus.Active
                                && l.Status == Constants.RecordStatus.Active)
                                select new LeaveModel
                                {
                                    Id = l.Id,
                                    EmployeeId = l.EmployeeId,
                                    RuleId = l.RuleId,
                                    Total = lr.MaxAllowedInYear,
                                    Credited = l.Credited,
                                    Applied = l.Applied,
                                    Available = l.Available,
                                    MinDate = currentDate.AddDays(-lr.MaxBackDatedLeavesAllowed),
                                    MaxDate = currentDate.AddDays(lr.FutureDatedLeavesAllowedUpTo)
                                }).ToListAsync();

            return leaves;
        }

        public async Task<List<Leave>> GetListByEmployeeIdAsync(int employeeId)
        {
            var leaves = await (from l in _dataContext.Leaves
                                join lr in _dataContext.LeaveRules on l.RuleId equals lr.Id
                                where (l.EmployeeId == employeeId
                                && lr.Status != Constants.RecordStatus.Deleted)
                                select new Leave
                                {
                                    Id = l.Id,
                                    EmployeeId = l.EmployeeId,
                                    RuleId = l.RuleId,
                                    Total = lr.MaxAllowedInYear,
                                    Credited = l.Credited,
                                    Applied = l.Applied,
                                    Available = l.Available
                                }).ToListAsync();

            return leaves;
        }

        public async Task<Leave> FindAsync(int id)
        {
            return await _dataContext.Leaves.FindAsync(id);
        }

        public void Update(Leave entity)
        {
            _dataContext.Leaves.Update(entity);
        }


        public decimal CalculateLeavesToCredit(DateTime joiningDate, int totalLeave, int leaveCredited = 0)
        {
            var currentDatetime = Utility.GetDateTime();
            if (joiningDate.Year < currentDatetime.Year)
            {
                joiningDate = new DateTime(currentDatetime.Year, 1, 1);
            }

            var leavePerMonth =  (decimal)totalLeave / 12;

            var monthCountToCalculate = (currentDatetime.Month - joiningDate.Month) + 1;

            var leaveCountToCredit = (monthCountToCalculate * leavePerMonth) - leaveCredited;

            return leaveCountToCredit;
        }


        public decimal CalculateLossOfPayCreditLeaves()
        {
            var currentDatetime = Utility.GetDateTime();
            var lossOfPayLeaveCount = DateTime.DaysInMonth(currentDatetime.Year, currentDatetime.Month);
            return lossOfPayLeaveCount;
        }
    }
}
