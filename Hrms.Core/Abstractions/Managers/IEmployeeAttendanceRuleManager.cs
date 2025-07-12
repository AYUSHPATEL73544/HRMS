using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IEmployeeAttendanceRuleManager
    {
        Task AddAsync(EmployeeAttendanceModel model);
        Task<MatTableResponse<EmployeeAttendanceModel>> GetpagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<EmployeeAttendanceModel>> GetInActivepagedListAsync(MatDataTableRequest model);
    }
}
