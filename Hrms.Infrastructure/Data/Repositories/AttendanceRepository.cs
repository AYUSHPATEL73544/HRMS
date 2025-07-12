
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DataContext _dataContext;
        public AttendanceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Attendance entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<AttendanceModel> GetAsync(int id)
        {
            return await _dataContext.Attendances
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new AttendanceModel
                {
                    EmployeeId = x.EmployeeId,
                    Date = x.Date,
                    DayOfWeek = x.DayOfWeek,
                    Logs = _dataContext.AttendanceLogs
                    .Select(y => new AttendanceLogModel
                    {
                        Id = y.Id,
                        InTime = y.InTime,
                        OutTime = y.OutTime,
                        AttendanceId = y.AttendanceId,
                        Latitude = y.Latitude,
                        Longitude = y.Longitude,
                        Status = y.Status
                    }).ToList(),
                    Status = x.Status
                }).SingleAsync();
        }

        public async Task<AttendanceModel> GetByEmployeeIdAsync(int employeeId, DateTime date)
        {
            return await _dataContext.Attendances
                 .Where(x => x.EmployeeId == employeeId
                 && x.Status != Constants.RecordStatus.Deleted
                 && x.Date.Date == date.Date)
                 .Select(x => new AttendanceModel
                 {
                     Id = x.Id
                 }).FirstOrDefaultAsync();


        }

        public async Task<List<AttendanceModel>> GetListAsync()
        {
            return await _dataContext.Attendances
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .Include(x => x.AttendanceLogs)
                .Select(x => new AttendanceModel
                {
                    EmployeeId = x.EmployeeId,
                    Date = x.Date,
                    DayOfWeek = x.DayOfWeek,
                    Status = x.Status
                }).ToListAsync();
        }

        public async Task<Attendance> FindAsync(int id)
        {
            return await _dataContext.Attendances.FindAsync(id);
        }

        public async Task<Attendance> GetByAsync(int id)
        {
            return await _dataContext.Attendances
                .Include(x => x.AttendanceLogs)
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .SingleAsync();
        }

        public void Update(Attendance entity)
        {
            _dataContext.Attendances.Update(entity);
        }
    }
}
