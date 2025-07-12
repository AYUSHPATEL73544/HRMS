using Hrms.Core.Entities;
using Hrms.Core.Models.Attendance;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IAttendanceRepository
    {
        Task AddAsync(Attendance entity);
        Task<AttendanceModel> GetAsync(int id);
        Task<List<AttendanceModel>> GetListAsync();
        Task<AttendanceModel> GetByEmployeeIdAsync(int employeeId, DateTime date);
        Task<Attendance> FindAsync(int id);
        Task<Attendance> GetByAsync(int id);
        void Update(Attendance entity);
    }
}
