using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class AttendanceRuleRepository : IAttendanceRuleRepository
    {
        private readonly DataContext _dataContext;
        public AttendanceRuleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(AttendanceRule entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<AttendanceRuleModel> GetAsync(int id)
        {
            return await _dataContext.AttendanceRules
                .Where(x => x.Id == id &&
                x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new AttendanceRuleModel
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Title = x.Title,
                    Description = x.Description,
                    InTime = x.InTime,
                    OutTime = x.OutTime,
                    FirstHalfStart = x.FirstHalfStart,
                    FirstHalfEnd = x.FirstHalfEnd,
                    SecondHalfStart = x.SecondHalfStart,
                    SecondHalfEnd = x.SecondHalfEnd,
                    GraceInTime = x.GraceInTime,
                    GraceOutTime = x.GraceOutTime,
                    TotalBreakDuration = x.TotalBreakDuration,
                    MinEffectiveDuration = x.MinEffectiveDuration,
                    AutoLeaveDeduction = x.AutoLeaveDeduction,
                    MinAnomaliesForFistHalfDeduction = x.MinAnomaliesForFistHalfDeduction,
                    MinAnomaliesForFullDayDeduction = x.MinAnomaliesForFullDayDeduction,
                    Status = x.Status,
                    StartDay = x.StartDay,
                    EndDay = x.EndDay,
                    NumberOfBreak = x.NumberOfBreaks,
                    Year = x.EffectiveFrom.Year
                }).SingleAsync();
        }

        public async Task<MatTableResponse<AttendanceRuleModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from a in _dataContext.AttendanceRules
                           where a.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(a.Title, "%" + model.FilterKey + "%"))
                           select new AttendanceRuleModel
                           {
                               Id = a.Id,
                               CompanyId = a.CompanyId,
                               Title = a.Title,
                               Description = a.Description,
                               InTime = a.InTime,
                               OutTime = a.OutTime,
                               FirstHalfStart = a.FirstHalfStart,
                               FirstHalfEnd = a.FirstHalfEnd,
                               SecondHalfStart = a.SecondHalfStart,
                               SecondHalfEnd = a.SecondHalfEnd,
                               GraceInTime = a.GraceInTime,
                               GraceOutTime = a.GraceOutTime,
                               TotalBreakDuration = a.TotalBreakDuration,
                               MinEffectiveDuration = a.MinEffectiveDuration,
                               AutoLeaveDeduction = a.AutoLeaveDeduction,
                               MinAnomaliesForFistHalfDeduction = a.MinAnomaliesForFistHalfDeduction,
                               MinAnomaliesForFullDayDeduction = a.MinAnomaliesForFullDayDeduction,
                               Status = a.Status,
                               StartDay = a.StartDay,
                               EndDay = a.EndDay,
                               NumberOfBreak = a.NumberOfBreaks,
                               Year = a.EffectiveFrom.Year
                           };

            var response = new MatTableResponse<AttendanceRuleModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                    .OrderBy(sortExpression)
                        .Skip(recordsToSkip)
                        .Take(model.PageSize)
                        .ToListAsync()
            };

            foreach(var items in response.Items)
            {
                items.Peoples = await (from a in _dataContext.EmployeeAttendanceRules
                                       join e in _dataContext.Employees on a.EmployeeId equals e.Id
                                       where a.AttendanceRuleId == items.Id
                                          && e.Status == Constants.RecordStatus.Active
                                          && a.Status == Constants.RecordStatus.Active
                                       select a.Id).CountAsync();
            }
            return response;
        }

        public async Task<MatTableResponse<AttendanceRuleModel>> GetPagedListAsync(MatDataTableRequest model, int employeeId)
        {
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from a in _dataContext.AttendanceRules
                           join empAtt in _dataContext.EmployeeAttendanceRules on a.Id equals empAtt.AttendanceRuleId
                           join e in _dataContext.Employees on empAtt.EmployeeId equals e.Id
                           where e.Id == employeeId &&
                           a.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(a.Title, "%" + model.FilterKey + "%"))
                           select new AttendanceRuleModel
                           {
                               Id = a.Id,
                               CompanyId = a.CompanyId,
                               Title = a.Title,
                               Description = a.Description,
                               InTime = a.InTime,
                               OutTime = a.OutTime,
                               FirstHalfStart = a.FirstHalfStart,
                               FirstHalfEnd = a.FirstHalfEnd,
                               SecondHalfStart = a.SecondHalfStart,
                               SecondHalfEnd = a.SecondHalfEnd,
                               GraceInTime = a.GraceInTime,
                               GraceOutTime = a.GraceOutTime,
                               TotalBreakDuration = a.TotalBreakDuration,
                               MinEffectiveDuration = a.MinEffectiveDuration,
                               AutoLeaveDeduction = a.AutoLeaveDeduction,
                               MinAnomaliesForFistHalfDeduction = a.MinAnomaliesForFistHalfDeduction,
                               MinAnomaliesForFullDayDeduction = a.MinAnomaliesForFullDayDeduction,
                               Status = a.Status,
                               StartDay = a.StartDay,
                               EndDay = a.EndDay,
                               NumberOfBreak = a.NumberOfBreaks,
                               Year = a.EffectiveFrom.Year
                           };

            var response = new MatTableResponse<AttendanceRuleModel>
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


        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _dataContext.AttendanceRules
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.Title
                }).ToListAsync();
        }

        public async Task<MatTableResponse<AttendanceRuleListItemModel>> GetPagedResultAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from a in _dataContext.AttendanceRules
                           where a.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(a.Title, "%" + model.FilterKey + "%"))
                           select new AttendanceRuleListItemModel
                           {
                               ShiftName = a.Title
                           };

            var response = new MatTableResponse<AttendanceRuleListItemModel>
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


        public async Task<AttendanceRule> FindAsync(int id)
        {
            return await _dataContext.AttendanceRules.FindAsync(id);
        }

        public void Update(AttendanceRule entity)
        {
            _dataContext.AttendanceRules.Update(entity);
        }

        public async Task<AttendanceRuleModel> GetByYearAsync(int year)
        {
            var res = await (from a in _dataContext.AttendanceRules
                             where (a.EffectiveFrom.Year == year
                             && a.Status != Constants.RecordStatus.Deleted)
                             select new AttendanceRuleModel
                             {
                                 Id = a.Id,
                                 CompanyId = a.CompanyId,
                                 Title = a.Title,
                                 Description = a.Description,
                                 InTime = a.InTime,
                                 OutTime = a.OutTime,
                                 FirstHalfStart = a.FirstHalfStart,
                                 FirstHalfEnd = a.FirstHalfEnd,
                                 SecondHalfStart = a.SecondHalfStart,
                                 SecondHalfEnd = a.SecondHalfEnd,
                                 GraceInTime = a.GraceInTime,
                                 GraceOutTime = a.GraceOutTime,
                                 TotalBreakDuration = a.TotalBreakDuration,
                                 MinEffectiveDuration = a.MinEffectiveDuration,
                                 AutoLeaveDeduction = a.AutoLeaveDeduction,
                                 MinAnomaliesForFistHalfDeduction = a.MinAnomaliesForFistHalfDeduction,
                                 MinAnomaliesForFullDayDeduction = a.MinAnomaliesForFullDayDeduction,
                                 Status = a.Status,
                                 StartDay = a.StartDay,
                                 EndDay = a.EndDay,
                                 NumberOfBreak = a.NumberOfBreaks,
                                 Year = a.EffectiveFrom.Year
                             }).SingleOrDefaultAsync();
            return res;
        }

        public async Task<bool> IsExistsAsync(int year)
        {
            var res = await _dataContext.AttendanceRules
                            .Where(x => x.EffectiveFrom.Year == year)
                            .Select(x => x.Title).AnyAsync();
            return res;
        }
    }
}

