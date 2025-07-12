using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IEmployeeAttendanceRuleRepository
    {
        Task AddAsync(EmployeeAttendanceRule entity);
        Task<MatTableResponse<EmployeeAttendanceModel>> GetpagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<EmployeeAttendanceModel>> GetInActivepagedListAsync(MatDataTableRequest model);
        Task<bool> IsExistAsync(int employeeId);
        Task<EmployeeAttendanceRule> GetByEmployeeIdAsync(int employeeId );
        Task<List<EmployeeAttendanceRule>> GetByRuleIdAsync(int ruleId);
        void Update(EmployeeAttendanceRule entity);
    }
}
