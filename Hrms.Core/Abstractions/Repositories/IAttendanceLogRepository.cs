using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IAttendanceLogRepository
    {
        Task AddAsync(AttendanceLog entity);
        Task<AttendanceLogModel> GetAsync(int id);
        Task<List<AttendanceLogModel>> GetAttendanceDeatilByEmployeeId(int id);
        Task<AttendanceLogModel> GetDeatilByAttendanceIdAsync(int attendanceId);
        Task<List<AttendanceModel>> GetListAsync();
        Task<AttendanceLogModel> GetDetailAsync(int userId);
        Task<MatTableResponse<AttendanceLogModel>> GetAttendancDetailsForEmployeeAsync(AttedanceFilterModel model, int attendanceId);
        Task<MatTableResponse<AttendanceLogModel>> GetEmployeeAttendanceHistoryAsync(AttedanceFilterModel model, int userId);
        Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryByEmployeeId(AttedanceFilterModel model,int id, int userId);
        Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryAsync(AttedanceFilterModel model);
        Task<AttendanceLog> FindAsync(int id);
        Task<AttendanceLog> FindByAttendanceAsync(int attendanceId);
        void Update(AttendanceLog entity);
        Task<MatTableResponse<AttendanceModel>> GetPagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<AttendanceModel>> GetPagedListForEmployeeAsync(MatDataTableRequest model, List<int> employeeIds, int userId);
        Task<List<AttendanceEventModel>> GetAttendanceLogEventAsync(int year, int month, int employeeId);
        Task<List<AttendanceEventModel>> GetAbsentEventAsync(int year, int month, int employeeId);
        bool IsHolidayExists(DateTime date);
    }
}
