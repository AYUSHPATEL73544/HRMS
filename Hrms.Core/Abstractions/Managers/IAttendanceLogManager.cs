using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IAttendanceLogManager
    {
        Task AddAsync(AttendanceLogModel model, int userId);
        Task ClockInAsync(AttendanceLogModel model, int userId);
        Task ClockOutAsync(AttendanceLogModel model, int userId);
        Task<AttendanceLogModel> GetAsync(int id);
        Task<List<AttendanceLogModel>> GetAttendanceDeatilByEmployeeId(int id);
        Task<List<AttendanceModel>> GetListAsync();
        Task<MatTableResponse<AttendanceLogModel>> GetAttendancDetailsForEmployeeAsync(AttedanceFilterModel model, int attendanceId);
        Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryByEmployeeId(AttedanceFilterModel model,int id, int userId);
        Task<MatTableResponse<AttendanceLogModel>> GetEmployeeAttendanceHistoryAsync(AttedanceFilterModel model, int userId);
        Task<AttendanceLogModel> GetDetailAsync(int userId);
        Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryAsync(AttedanceFilterModel model);
        Task UpdateAsync(AttendanceLogModel model, int userId);
        Task DeleteAttendanceLogAsync(int id);
        Task DeleteAttendanceAsync(int id);
        Task<MatTableResponse<AttendanceModel>> GetPagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<AttendanceModel>> GetPagedListForEmployeeAsync(MatDataTableRequest model, int userId);
        Task<List<AttendanceEventModel>> GetAttendanceLogEventAsync(int yesr, int month, int employeeId);
        Task<List<AttendanceEventModel>> GetEmployeeAttendanceLogEventAsync(int year, int month, int userId);
        Task<List<AttendanceEventModel>> GetAbsentEventAsync(int year, int month, int employeeId);
        Task<List<AttendanceEventModel>> GetEmployeeAbsentEventAsync(int year, int month, int userId);
        
    }
}
