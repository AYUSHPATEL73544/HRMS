using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Security.Policy;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class LeaveLogRepository : ILeaveLogRepository
    {
        private readonly DataContext _dataContext;
        public LeaveLogRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(LeaveLog entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<LeaveLogModel> GetAsync(int id)
        {
            return await _dataContext.LeaveLogs
               .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new LeaveLogModel
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    RuleId = x.RuleId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    StartHalf = x.StartHalf,
                    EndHalf = x.EndHalf,
                    Purpose = x.Purpose,
                    Status = x.Status,
                    CreatedOn = x.CreatedOn,
                    Days = x.Days
                }).SingleAsync();
        }

        public async Task<MatTableResponse<LeaveLogModel>> PagedListByEmployeeIdAsync(LeaveLogFilterModel model, int employeeId)
        {
            var currentDate = Utility.GetDateTime();
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from l in _dataContext.LeaveLogs
                           join lr in _dataContext.LeaveRules on l.RuleId equals lr.Id
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           where l.EmployeeId == employeeId
                           && l.EffectiveFrom <= currentDate
                           && l.Status != Constants.RecordStatus.Deleted
                           && (l.EffectiveTo == null || l.EffectiveTo >= currentDate)
                           && (model.StartDate == null || l.StartDate >= model.StartDate)
                           && (model.EndDate == null || l.EndDate <= model.EndDate)
                           && (model.FilterKey == null
                           || EF.Functions.Like(lr.Title, "%" + model.FilterKey + "%"))
                           select new LeaveLogModel
                           {
                               Id = l.Id,
                               EmployeeId = l.EmployeeId,
                               RuleId = lr.Id,
                               LeaveType = lr.Title,
                               EndDate = l.EndDate,
                               StartDate = l.StartDate,
                               StartHalf = l.StartHalf,
                               EndHalf = l.EndHalf,
                               Purpose = l.Purpose,
                               Status = l.Status,
                               CreatedOn = l.CreatedOn,
                               Days = l.Days,
                               RejectionReason = l.RejectionReason,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               EmployeeCode = e.Code,
                               MinDate = currentDate.AddDays(-lr.MaxBackDatedLeavesAllowed),
                               MaxDate = currentDate.AddDays(lr.FutureDatedLeavesAllowedUpTo)
                           };

            var response = new MatTableResponse<LeaveLogModel>
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

        public async Task<MatTableResponse<LeaveLogModel>> GetPagedListAsync(LeaveLogFilterModel model)
        {
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from l in _dataContext.LeaveLogs
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           join r in _dataContext.LeaveRules on l.RuleId equals r.Id
                           where l.Status != Constants.RecordStatus.Deleted
                           && ((model.StartDate == null || l.StartDate >= model.StartDate)
                           && (model.EndDate == null || l.EndDate <= model.EndDate))
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%"))

                           select new LeaveLogModel
                           {
                               Id = l.Id,
                               EmployeeId = e.Id,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               EmployeeCode = e.Code,
                               LeaveType = r.Title,
                               RuleId = l.RuleId,
                               CreatedOn = l.CreatedOn,
                               StartDate = l.StartDate,
                               EndDate = l.EndDate,
                               StartHalf = l.StartHalf,
                               EndHalf = l.EndHalf,
                               Purpose = l.Purpose,
                               Status = l.Status,
                               EmployeeStatus = e.Status,
                               Days = l.Days,
                               RejectionReason = l.RejectionReason

                           };

            var response = new MatTableResponse<LeaveLogModel>
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

        public async Task<MatTableResponse<LeaveLogModel>> GetReporteePagedListAsync(LeaveLogFilterModel model, int userId)
        {
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var managerId = await (from u in _dataContext.Users
                                   join e in _dataContext.Employees on u.Id equals e.UserId
                                   where u.Id == userId
                                   && u.Status != Constants.RecordStatus.Deleted
                                   && e.Status != Constants.RecordStatus.Deleted
                                   select e.Id).SingleAsync();

            var linqStmt = from l in _dataContext.LeaveLogs
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           join r in _dataContext.LeaveRules on l.RuleId equals r.Id
                           join t in _dataContext.Teams on e.Id equals t.EmployeeId
                           where l.Status != Constants.RecordStatus.Deleted
                           && t.Status != Constants.RecordStatus.Deleted
                           && t.ManagerId == managerId
                           && ((model.StartDate == null || l.StartDate >= model.StartDate)
                           && (model.EndDate == null || l.EndDate <= model.EndDate))
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%"))

                           select new LeaveLogModel
                           {
                               Id = l.Id,
                               EmployeeId = e.Id,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               EmployeeCode = e.Code,
                               LeaveType = r.Title,
                               RuleId = l.RuleId,
                               CreatedOn = l.CreatedOn,
                               StartDate = l.StartDate,
                               EndDate = l.EndDate,
                               StartHalf = l.StartHalf,
                               EndHalf = l.EndHalf,
                               Purpose = l.Purpose,
                               Status = l.Status,
                               EmployeeStatus = e.Status,
                               Days = l.Days,
                               RejectionReason = l.RejectionReason

                           };

            var response = new MatTableResponse<LeaveLogModel>
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

        public async Task<MatTableResponse<LeaveLogModel>> GetPendingLeavesPagedListAsync(LeaveLogFilterModel model)
        {
            var currentDate = Utility.GetDateTime();
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from l in _dataContext.LeaveLogs
                           join e in _dataContext.Employees on l.EmployeeId equals e.Id
                           join r in _dataContext.LeaveRules on l.RuleId equals r.Id
                           where l.Status == Constants.RecordStatus.Pending
                           && l.EffectiveFrom <= currentDate
                           && (l.EffectiveTo == null || l.EffectiveTo >= currentDate)
                           && ((model.StartDate == null || l.StartDate >= model.StartDate)
                           && (model.EndDate == null || l.EndDate <= model.EndDate))
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%"))
                           select new LeaveLogModel
                           {
                               Id = l.Id,
                               EmployeeId = e.Id,
                               EmployeeStatus = e.Status,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               EmployeeCode = e.Code,
                               LeaveType = r.Title,
                               RuleId = l.RuleId,
                               CreatedOn = l.CreatedOn,
                               StartDate = l.StartDate,
                               EndDate = l.EndDate,
                               StartHalf = l.StartHalf,
                               EndHalf = l.EndHalf,
                               Purpose = l.Purpose,
                               Status = l.Status,
                               Days = l.Days,
                               RejectionReason = l.RejectionReason
                           };

            var response = new MatTableResponse<LeaveLogModel>
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

        public async Task<bool> IsExistsAsync(int employeeId, DateTime startDate, DateTime endDate, int id)
        {
            var leaves = await _dataContext.LeaveLogs
                .AsNoTracking()
                .AnyAsync(x => x.EmployeeId == employeeId
                && ((startDate.Date <= x.EndDate.Date
                && endDate.Date >= x.StartDate.Date)
                || (x.StartDate.Date <= startDate.Date
                && x.EndDate.Date >= endDate.Date))
                && (x.Id != id)
                && (x.Status == Constants.RecordStatus.Approved || x.Status == Constants.RecordStatus.Pending));

            return leaves;
        }

        public async Task<List<LeaveLog>> GetByRuleIdAsync(int ruleId)
        {
            return await _dataContext.LeaveLogs
              .Where(x => x.RuleId == ruleId
              && x.Status != Constants.RecordStatus.Deleted)
              .Select(x => new LeaveLog
              {
                  Id = x.Id,
                  EmployeeId = x.EmployeeId,
                  RuleId = x.RuleId,
                  StartDate = x.StartDate,
                  EndDate = x.EndDate,
                  StartHalf = x.StartHalf,
                  EndHalf = x.EndHalf,
                  Purpose = x.Purpose,
                  Status = x.Status,
                  Days = x.Days
              }).ToListAsync();
        }

        public async Task<List<LeaveLog>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dataContext.LeaveLogs
              .Where(x => x.EmployeeId == employeeId
              && x.Status != Constants.RecordStatus.Deleted)
              .Select(x => new LeaveLog
              {
                  Id = x.Id,
                  EmployeeId = x.EmployeeId,
                  RuleId = x.RuleId,
                  StartDate = x.StartDate,
                  EndDate = x.EndDate,
                  StartHalf = x.StartHalf,
                  EndHalf = x.EndHalf,
                  Purpose = x.Purpose,
                  Status = x.Status,
                  Days = x.Days,
                  CreatedOn = x.CreatedOn
              }).ToListAsync();
        }

        public async Task<LeaveLog> FindAsync(int id)
        {
            return await _dataContext.LeaveLogs.FindAsync(id);
        }

        public async Task<LeaveLog> PreviousLeaveLogAsync(DateTime date, int employeeId, int ruleId)
        {
            var fridayDate = date.AddDays(-3);
            var response = await _dataContext.LeaveLogs
                                             .Where(x => x.EmployeeId == employeeId
                                                      && x.RuleId == ruleId
                                                      && x.EndDate == fridayDate
                                                      && (x.Status == Constants.RecordStatus.Active 
                                                      || x.Status == Constants.RecordStatus.Pending
                                                      || x.Status == Constants.RecordStatus.Approved))
                                             .Select(x => x).SingleOrDefaultAsync();
            return response;
        }

        public async Task<LeaveLog> GetPreviousLeaveLogDatesAsync(DateTime date, int employeeId, int ruleId)
        {
            var response = await _dataContext.LeaveLogs
                                        .Where(x => x.EmployeeId == employeeId
                                                 && x.RuleId == ruleId
                                                 && x.EndDate < date
                                                 && (x.Status == Constants.RecordStatus.Active
                                                 || x.Status == Constants.RecordStatus.Pending
                                                 || x.Status == Constants.RecordStatus.Approved))
                                        .OrderByDescending(x => x.EndDate)
                                        .Select(x => x).FirstOrDefaultAsync();
           
            return response;
        }

        public void Update(LeaveLog entity)
        {
            _dataContext.LeaveLogs.Update(entity);
        }

        public void Update(List<LeaveLog> entity)
        {
            _dataContext.LeaveLogs.UpdateRange(entity);
        }

        public async Task<decimal> GetMonthlyLeaveLogAsync(int employeeId, int ruleId, DateTime start)
        {
            var currentMonth = start.Month;
            return await _dataContext.LeaveLogs.Where(x => x.EmployeeId == employeeId
            && x.RuleId == ruleId
            && x.StartDate.Month == currentMonth
            && (x.Status == Constants.RecordStatus.Approved
                || x.Status == Constants.RecordStatus.Pending))
                .Select(x => x.Days)
                .SumAsync();
        }

        public async Task<List<LeaveLog>> GetByRuleIdEmployeeId(int ruleId, int employeeId)
        {
            return await _dataContext.LeaveLogs
          .Where(x => x.RuleId == ruleId
           && x.EmployeeId == employeeId
           && x.Status == Constants.RecordStatus.Pending)
          .ToListAsync();

        }

        public async Task<List<LeaveLogModel>> GetLeaveLog( DateTime startDate, DateTime endDate)
        {
            var res = await (from l in _dataContext.LeaveLogs 
                          join e in _dataContext.Employees  on l.EmployeeId equals e.Id 
                          where l.StartDate <= endDate && l.EndDate >= startDate 
                          && (l.Status == Constants.RecordStatus.Pending 
                          || l.Status == Constants.RecordStatus.Approved)
                          select new LeaveLogModel
                          {
                              Id = l.Id,
                              EmployeeId = e.Id,
                              EmployeeName = e.FirstName + " " + e.LastName,
                              StartDate = l.StartDate,
                              EndDate = l.EndDate,
                              Status = l.Status,
                          }).ToListAsync();
            return res;
          
        }

    } 
}
