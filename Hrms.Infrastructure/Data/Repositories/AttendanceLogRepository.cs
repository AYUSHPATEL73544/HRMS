using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class AttendanceLogRepository : IAttendanceLogRepository
    {
        private readonly DataContext _dataContext;
        public AttendanceLogRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(AttendanceLog entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<AttendanceLogModel> GetAsync(int id)

        {
            return await (from alog in _dataContext.AttendanceLogs
                          join a in _dataContext.Attendances on alog.AttendanceId equals a.Id
                          where (alog.Id == id
                          && alog.Status != Constants.RecordStatus.Deleted)
                          select new AttendanceLogModel
                          {
                              Id = alog.Id,
                              Status = alog.Status,
                              InTime = alog.InTime,
                              OutTime = alog.OutTime,
                              Note = alog.Note,
                              Date = a.Date,
                              Longitude = alog.Longitude,
                              Latitude = alog.Latitude,

                          }).SingleOrDefaultAsync();
        }

        public async Task<List<AttendanceLogModel>> GetAttendanceDeatilByEmployeeId(int id)
        {
            return await (from attlog in _dataContext.AttendanceLogs
                          join a in _dataContext.Attendances on attlog.AttendanceId equals a.Id
                          join e in _dataContext.Employees on a.EmployeeId equals e.Id
                          where (e.Id == id && a.Date.Date == Utility.GetDateTime().Date
                          && attlog.Status != Constants.RecordStatus.Deleted)
                          select new AttendanceLogModel
                          {
                              Id = attlog.Id,
                              InTime = attlog.InTime,
                              OutTime = attlog.OutTime,
                              Longitude = attlog.Longitude,
                              Latitude = attlog.Latitude,
                              Date = a.Date,
                              Note = attlog.Note,
                              EmployeeId = e.Id,
                              EmployeeName = e.FirstName + " " + e.LastName,
                              EmployeeCode = e.Code
                          }).ToListAsync();
        }

        public async Task<AttendanceLogModel> GetDeatilByAttendanceIdAsync(int attendanceId)
        {
            return await _dataContext.AttendanceLogs
               .Where(x => x.AttendanceId == attendanceId
               && x.Status != Constants.RecordStatus.Deleted)
               .Select(x => new AttendanceLogModel
               {
                   Id = x.Id,
                   AttendanceId = x.AttendanceId,
                   InTime = x.InTime,
                   OutTime = x.OutTime,
                   Latitude = x.Latitude,
                   Longitude = x.Longitude,
                   Note = x.Note,
                   Status = x.Status
               }).SingleAsync();
        }

        public async Task<List<AttendanceModel>> GetListAsync()
        {
            var logs = await (from a in _dataContext.Attendances
                              join e in _dataContext.Employees on a.EmployeeId equals e.Id
                              join alog in _dataContext.AttendanceLogs on a.Id equals alog.AttendanceId
                              where (a.Status != Constants.RecordStatus.Deleted
                              && a.Date.Date == Utility.GetDateTime().Date)
                              group new { alog, a, e } by a.Id into g
                              select new AttendanceModel
                              {
                                  Id = g.Key,
                                  EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                                  Date = g.Select(x => x.a.Date).FirstOrDefault(),
                                  EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                                  EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                                  Logs = g.Select(x => new AttendanceLogModel
                                  {
                                      InTime = x.alog.InTime,
                                      OutTime = x.alog.OutTime,
                                      Date = x.a.Date,
                                      Note = x.alog.Note
                                  }).ToList()
                              }).ToListAsync();

            foreach (var group in logs)
            {
                var totalWorkDuration = TimeSpan.Zero;
                foreach (var log in group.Logs)
                {
                    if (log.OutTime != null)
                    {
                        var workHours = log.OutTime.Value - log.InTime;
                        totalWorkDuration += workHours;
                    }
                }

                group.WorkDuration = totalWorkDuration;
            }

            return logs;
        }

        public async Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryByEmployeeId(AttedanceFilterModel model, int id, int userId)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from attlog in _dataContext.AttendanceLogs
                           join a in _dataContext.Attendances on attlog.AttendanceId equals a.Id
                           join e in _dataContext.Employees on a.EmployeeId equals e.Id
                           where (
                            a.Id == id
                           && attlog.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null)
                           && (model.StartDate == null || a.Date >= model.StartDate)
                           && (model.EndDate == null || a.Date.Date <= model.EndDate))
                           select new AttendanceLogModel
                           {
                               Id = attlog.Id,
                               InTime = attlog.InTime,
                               OutTime = attlog.OutTime,
                               Longitude = attlog.Longitude,
                               Latitude = attlog.Latitude,
                               Date = a.Date,
                               Note = attlog.Note,
                               EmployeeId = e.Id,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               EmployeeCode = e.Code
                           };

            var response = new MatTableResponse<AttendanceLogModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt.OrderBy(sortExpression).
                                       Skip(recordsToSkip).
                                       Take(model.PageSize).
                                       ToListAsync()
            };
            return response;
        }


        public async Task<MatTableResponse<AttendanceLogModel>> GetEmployeeAttendanceHistoryAsync(AttedanceFilterModel model, int userId)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var reportingEmployees = await (from e in _dataContext.Employees
                                            join t in _dataContext.Teams on e.Id equals t.ManagerId
                                            where e.UserId == userId 
                                            && t.Status != Constants.RecordStatus.Deleted
                                            select t.EmployeeId)
                                          .ToListAsync();

            var linqStmt = from attlog in _dataContext.AttendanceLogs
                           join a in _dataContext.Attendances on attlog.AttendanceId equals a.Id
                           join e in _dataContext.Employees on a.EmployeeId equals e.Id
                           where ((e.UserId == userId
                           || reportingEmployees.Contains(e.Id))
                           && attlog.Status != Constants.RecordStatus.Deleted
                           && ((model.StartDate == null || a.Date >= model.StartDate)
                           && (model.EndDate == null || a.Date <= model.EndDate))
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%")))
                           group new { attlog, a, e } by a.Id into g
                           select new AttendanceLogModel
                           {
                               Id = g.Key,
                               InTime = g.Select(x => x.attlog.InTime).FirstOrDefault(),
                               OutTime = g.Select(x => x.attlog.OutTime).FirstOrDefault(),
                               Longitude = g.Select(x => x.attlog.Longitude).FirstOrDefault(),
                               Latitude = g.Select(x => x.attlog.Latitude).FirstOrDefault(),
                               Date = g.Select(x => x.a.Date).FirstOrDefault(),
                               Note = g.Select(x => x.attlog.Note).FirstOrDefault(),
                               EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                               EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                               EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                               AttendanceId = g.Select(x => x.a.Id).FirstOrDefault()
                           };

            var response = new MatTableResponse<AttendanceLogModel>
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

        public async Task<MatTableResponse<AttendanceLogModel>> GetAttendancDetailsForEmployeeAsync(AttedanceFilterModel model, int attendanceId)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from attlog in _dataContext.AttendanceLogs
                           join a in _dataContext.Attendances on attlog.AttendanceId equals a.Id
                           join e in _dataContext.Employees on a.EmployeeId equals e.Id
                           join ea in _dataContext.EmployeeAttendanceRules on e.Id equals ea.EmployeeId
                           join ar in _dataContext.AttendanceRules on ea.AttendanceRuleId equals ar.Id
                           where (a.Id == attendanceId
                           && attlog.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null)
                           && (model.StartDate == null || a.Date >= model.StartDate)
                           && (model.EndDate == null || a.Date.Date <= model.EndDate)
                           && (model.InTime == null || attlog.InTime >= model.InTime))
                           select new AttendanceLogModel
                           {
                               Id = attlog.Id,
                               InTime = attlog.InTime,
                               OutTime = attlog.OutTime,
                               Longitude = attlog.Longitude,
                               Latitude = attlog.Latitude,
                               Date = a.Date,
                               Note = attlog.Note,
                               EmployeeId = e.Id,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               EmployeeCode = e.Code,
                               GraceInTime = ar.GraceInTime
                           };

            var response = new MatTableResponse<AttendanceLogModel>
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

        public async Task<AttendanceLogModel> GetDetailAsync(int userId)
        {
            var attendance = await (from e in _dataContext.Employees
                                    join a in _dataContext.Attendances on e.Id equals a.EmployeeId
                                    join al in _dataContext.AttendanceLogs on a.Id equals al.AttendanceId
                                    where (e.UserId == userId && al.OutTime == null && a.Date.Date == Utility.GetDateTime().Date
                                    && al.Status != Constants.RecordStatus.Deleted)
                                    select new AttendanceLogModel
                                    {
                                        Id = al.Id,
                                        AttendanceId = a.Id,
                                        InTime = al.InTime,
                                        OutTime = al.OutTime,
                                        Status = al.Status,
                                        Note = al.Note,
                                        Date = a.Date
                                    }).FirstOrDefaultAsync();

            return attendance;
        }

        public async Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryAsync(AttedanceFilterModel model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from attlog in _dataContext.AttendanceLogs
                           join a in _dataContext.Attendances on attlog.AttendanceId equals a.Id
                           join e in _dataContext.Employees on a.EmployeeId equals e.Id
                           join ea in _dataContext.EmployeeAttendanceRules on e.Id equals ea.EmployeeId
                           join ar in _dataContext.AttendanceRules on ea.AttendanceRuleId equals ar.Id
                           where attlog.Status != Constants.RecordStatus.Deleted
                           && ((model.StartDate == null || a.Date >= model.StartDate)
                           && (model.EndDate == null || a.Date <= model.EndDate.Value.AddDays(1))
                           && (model.InTime == null || attlog.InTime >= model.InTime))
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%"))
                           group new { attlog, a, e, ar } by a.Id into g
                           select new AttendanceLogModel
                           {
                               Id = g.Key,
                               InTime = g.Select(x => x.attlog.InTime).FirstOrDefault(),
                               OutTime = g.Select(x => x.attlog.OutTime).FirstOrDefault(),
                               Longitude = g.Select(x => x.attlog.Longitude).FirstOrDefault(),
                               Latitude = g.Select(x => x.attlog.Latitude).FirstOrDefault(),
                               Date = g.Select(x => x.a.Date).FirstOrDefault(),
                               Note = g.Select(x => x.attlog.Note).FirstOrDefault(),
                               AttendanceId = g.Select(x => x.a.Id).FirstOrDefault(),
                               EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                               EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                               EmployeeStatus = g.Select(x => x.e.Status).FirstOrDefault(),
                               EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                               GraceInTime = g.Select(x => x.ar.GraceInTime).FirstOrDefault(),
                           };

            var response = new MatTableResponse<AttendanceLogModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };

            foreach (var item in response.Items)
            {
                if (item.OutTime != null)
                {
                    TimeSpan workDuration = item.OutTime.Value - item.InTime;
                    item.WorkDuration = (int)workDuration.TotalHours;
                    if (item.WorkDuration < 9)
                    {
                        item.IsWorkDurationLess = true;
                    }
                }
            }
            return response;
        }

        public async Task<AttendanceLog> FindAsync(int id)
        {
            return await _dataContext.AttendanceLogs.FindAsync(id);
        }

        public async Task<AttendanceLog> FindByAttendanceAsync(int attendanceId)
        {
            return await _dataContext.AttendanceLogs.Where(x => x.AttendanceId == attendanceId
            && x.OutTime == null)
                .Select(x => new AttendanceLog
                {
                    Id = x.Id,
                    AttendanceId = x.AttendanceId,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    InTime = x.InTime,
                    OutTime = x.OutTime,
                    Status = x.Status,
                    Note = x.Note
                }).FirstOrDefaultAsync();
        }

        public void Update(AttendanceLog entity)
        {
            _dataContext.AttendanceLogs.Update(entity);
        }

        public async Task<MatTableResponse<AttendanceModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = await (from a in _dataContext.Attendances
                                  join e in _dataContext.Employees on a.EmployeeId equals e.Id
                                  join alog in _dataContext.AttendanceLogs on a.Id equals alog.AttendanceId
                                  join eattnd in _dataContext.EmployeeAttendanceRules on e.Id equals eattnd.EmployeeId into eattndGroup
                                  from eattnd in eattndGroup.DefaultIfEmpty()
                                  join attrule in _dataContext.AttendanceRules on eattnd.AttendanceRuleId equals attrule.Id into attruleGroup
                                  from attrule in attruleGroup.DefaultIfEmpty()
                                  where a.Status != Constants.RecordStatus.Deleted
                                        && a.Date.Date == Utility.GetDateTime().Date
                                        && (model.FilterKey == null
                                            || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                                            || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                                            || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                                            || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%"))
                                  group new { alog, a, e, attrule } by a.Id into g
                                  select new AttendanceModel
                                  {
                                      Id = g.Key,
                                      EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                                      Date = g.Select(x => x.a.Date).FirstOrDefault(),
                                      FirstClockIn = g.Select(x => x.alog.InTime).FirstOrDefault(),
                                      LastClockOut = g.Select(x => x.alog.OutTime).FirstOrDefault(),
                                      EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                                      EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                                      Logs = g.Select(x => new AttendanceLogModel
                                      {
                                          Id = x.alog.Id,
                                          AttendanceId = x.alog.AttendanceId,
                                          InTime = x.alog.InTime,
                                          OutTime = x.alog.OutTime,
                                          Latitude = x.alog.Latitude,
                                          Note = x.alog.Note,
                                          Longitude = x.alog.Longitude,
                                          Date = x.a.Date,
                                          Status = x.alog.Status,
                                          EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                                          EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                                          EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                                      }).ToList(),
                                      AttendanceRule = g.Select(x => x.attrule).FirstOrDefault()
                                  }).OrderBy(sortExpression)
                                    .Skip(recordsToSkip)
                                    .ToListAsync();

            var response = new MatTableResponse<AttendanceModel>
            {
                TotalCount = linqStmt.Count,
                Items = linqStmt.Take(model.PageSize)
            };

            foreach (var item in response.Items)
            {
                var totalWorkDuration = TimeSpan.Zero;
                TimeSpan? firstClockIn = null;
                TimeSpan? lastClockOut = null;
                bool isWorkDurationLess = false;

                foreach (var log in item.Logs)
                {
                    if (!firstClockIn.HasValue || log.InTime < firstClockIn.Value)
                    {
                        firstClockIn = log.InTime;
                    }
                    if (log.OutTime.HasValue)
                    {
                        var workHours = log.OutTime.Value - log.InTime;
                        totalWorkDuration += workHours;

                        if (!lastClockOut.HasValue || log.OutTime.Value > lastClockOut.Value)
                        {
                            lastClockOut = log.OutTime.Value;
                        }
                    }
                }
                if (totalWorkDuration < TimeSpan.FromHours(9)) // Check if total work duration is less than 9 hours
                {
                    isWorkDurationLess = true; // Set the flag to true
                }

                item.WorkDuration = totalWorkDuration;
                item.FirstClockIn = firstClockIn;
                item.LastClockOut = lastClockOut;
                item.IsWorkDurationLess = isWorkDurationLess;
            }
            return response;
        }

        public async Task<MatTableResponse<AttendanceModel>> GetPagedListForEmployeeAsync(MatDataTableRequest model, List<int> employeeIds, int userId)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = await (from a in _dataContext.Attendances
                                  join e in _dataContext.Employees on a.EmployeeId equals e.Id
                                  join alog in _dataContext.AttendanceLogs on a.Id equals alog.AttendanceId
                                  where (e.UserId == userId || employeeIds.Contains(e.Id))
                                  && a.Status != Constants.RecordStatus.Deleted
                                  && a.Date.Date == Utility.GetDateTime().Date
                                  && (model.FilterKey == null
                                  || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                                  || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                                  || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                                  || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%"))
                                  group new { alog, a, e } by a.Id into g
                                  select new AttendanceModel
                                  {
                                      Id = g.Key,
                                      EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                                      Date = g.Select(x => x.a.Date).FirstOrDefault(),
                                      FirstClockIn = g.Select(x => x.alog.InTime).FirstOrDefault(),
                                      LastClockOut = g.Select(x => x.alog.OutTime).FirstOrDefault(),
                                      EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                                      EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                                      Logs = g.Select(x => new AttendanceLogModel
                                      {
                                          Id = x.alog.Id,
                                          AttendanceId = x.alog.AttendanceId,
                                          InTime = x.alog.InTime,
                                          OutTime = x.alog.OutTime,
                                          Latitude = x.alog.Latitude,
                                          Longitude = x.alog.Longitude,
                                          Note = x.alog.Note,
                                          Date = x.a.Date,
                                          Status = x.alog.Status,
                                          EmployeeName = g.Select(x => x.e.FirstName).FirstOrDefault() + " " + g.Select(x => x.e.LastName).FirstOrDefault(),
                                          EmployeeId = g.Select(x => x.e.Id).FirstOrDefault(),
                                          EmployeeCode = g.Select(x => x.e.Code).FirstOrDefault(),
                                      }).ToList()
                                  }).OrderBy(sortExpression)
                .Skip(recordsToSkip)
                .ToListAsync();

            var response = new MatTableResponse<AttendanceModel>
            {
                TotalCount = linqStmt.Count,
                Items = linqStmt.Take(model.PageSize)
            };

            foreach (var item in response.Items)
            {
                var totalWorkDuration = TimeSpan.Zero;
                TimeSpan? firstClockIn = null;
                TimeSpan? lastClockOut = null;

                foreach (var log in item.Logs)
                {
                    if (!firstClockIn.HasValue || log.InTime < firstClockIn.Value)
                    {
                        firstClockIn = log.InTime;
                    }
                    if (log.OutTime.HasValue)
                    {
                        var workHours = log.OutTime.Value - log.InTime;
                        totalWorkDuration += workHours;

                        if (!lastClockOut.HasValue || log.OutTime.Value > lastClockOut.Value)
                        {
                            lastClockOut = log.OutTime.Value;
                        }
                    }
                }
                item.WorkDuration = totalWorkDuration;
                item.FirstClockIn = firstClockIn;
                item.LastClockOut = lastClockOut;
            }
            return response;
        }

        public async Task<List<AttendanceEventModel>> GetAttendanceLogEventAsync(int year, int month,int employeeId)
        {
            var res = await (from a in _dataContext.Attendances
                             join e in _dataContext.Employees on a.EmployeeId equals e.Id
                             join alog in _dataContext.AttendanceLogs on a.Id equals alog.AttendanceId
                             join attr in _dataContext.EmployeeAttendanceRules on e.Id equals attr.EmployeeId into attrule
                             from attenrule in attrule.DefaultIfEmpty()
                             where (a.Date.Year == year && a.Date.Month == month 
                                    && e.Id == employeeId)
                             group new { a, alog, e, attenrule } by a.Id into g
                             select new AttendanceEventModel
                             {
                                 FirstClockIn = g.Select(x => x.alog.InTime).FirstOrDefault(),
                                 LastClockOut = g.Max(x => x.alog.OutTime),
                                 Date = g.Select(x => x.a.Date).FirstOrDefault(),
                                 AttendanceRuleId = g.Select(x => x.attenrule.Id).SingleOrDefault(),
                                 Logs = g.Select(x => new AttendanceLogModel
                                 {
                                     AttendanceId = x.alog.Id, 
                                     InTime = x.alog.InTime,
                                     OutTime = x.alog.OutTime
                                 }).ToList()
                             }).ToListAsync();


            foreach(var item in res)
            {
                var totalWorkDuration = TimeSpan.Zero;
                TimeSpan? firstClockIn = null;
                TimeSpan? lastClockOut = null;

                foreach(var log in item.Logs)
                {
                    if(!firstClockIn.HasValue || log.InTime < firstClockIn.Value)
                    {
                        firstClockIn = log.InTime;
                    }
                    if(log.OutTime.HasValue)
                    {
                        var workHours = log.OutTime.Value - log.InTime;
                        totalWorkDuration += workHours;

                        if (!lastClockOut.HasValue || log.OutTime.Value > lastClockOut.Value)
                        {
                            lastClockOut = log.OutTime.Value;
                        }
                    }
                }
                item.WorkDuration = totalWorkDuration;
                item.FirstClockIn = firstClockIn;
                item.LastClockOut = lastClockOut;
            }
             
            return res;
        }


        public async Task<List<AttendanceEventModel>> GetAbsentEventAsync(int year, int month, int employeeId)
        {
            List<AttendanceEventModel> attendanceModel = new List<AttendanceEventModel>();
            if (month > DateTime.Now.Month && year >= DateTime.Now.Year )
            {
                return attendanceModel;
            }
            if (year < DateTime.Now.Year)
            {
                return attendanceModel;
            }

            var res = await (from a in _dataContext.Attendances
                             join e in _dataContext.Employees on a.EmployeeId equals e.Id
                             join eatrule in _dataContext.EmployeeAttendanceRules on e.Id equals eatrule.EmployeeId
                             join atrule in _dataContext.AttendanceRules on eatrule.AttendanceRuleId equals atrule.Id
                             where (a.Date.Year == year && a.Date.Month == month
                             && e.Id == employeeId
                             && a.Status != Constants.RecordStatus.Deleted)
                             group new { a, e, atrule, eatrule } by a.Id into g
                             select new AttendanceEventModel
                             {
                                 Date = g.Select(x => x.a.Date).FirstOrDefault(),
                                 EndDay = g.Select(x => x.atrule.EndDay).FirstOrDefault(),
                             }).ToListAsync();


            var lastWorkingDay = res.Select(x => x.EndDay).FirstOrDefault();
            var today = DateTime.Now;
            var firstDateOfMonth = new DateTime(year, month, 1);
            var lastDatOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);

            var dateList = new List<DateTime>();
            if(month == DateTime.Now.Month)
            {
                dateList = GetDateList(firstDateOfMonth, today);
            }
            else
            {
                dateList = GetDateList(firstDateOfMonth, lastDatOfMonth);
            }

            foreach(var date in dateList)
            {
                if(IsHolidayExists(date) ||  date.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                var leave = _dataContext.LeaveLogs.Where(x => x.Status == Constants.RecordStatus.Approved
                                                            && x.StartDate <= date
                                                            && x.EndDate >= date
                                                            && x.EmployeeId == employeeId).FirstOrDefault();

                if (leave != null)
                {
                    if (date <= leave.EndDate && date >= leave.StartDate)
                    {
                        continue;
                    }
                }

                var attendance = res.FirstOrDefault(x => x.Date == date);
                if(attendance == null)
                {
                    if (lastWorkingDay != (int)Constants.DaysOfWeek.Saturday && date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        continue;
                    }
                    var absentModel = new AttendanceEventModel
                    {
                        Date = date,
                    };
                    attendanceModel.Add(absentModel);
                }
            }
            return attendanceModel;
        }


        public bool IsHolidayExists(DateTime date)
        {
            var res =  _dataContext.Holidays
                            .Where(x => x.Date == date
                             && x.Status != Constants.RecordStatus.Deleted)
                            .Select(x => x.Name).Any();
            return res;
        }
        
        public List<DateTime> GetDateList(DateTime startDate, DateTime endDate)
        {
            List<DateTime> dateList = new List<DateTime>();
            for(var date=startDate; date < endDate; date = date.AddDays(1))
            {
                dateList.Add(date);
            }
            return dateList;
        }
    }
}
