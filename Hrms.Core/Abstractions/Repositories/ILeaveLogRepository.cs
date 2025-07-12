using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface ILeaveLogRepository
    {
        Task AddAsync(LeaveLog entity);
        Task<LeaveLogModel> GetAsync(int id);
        Task<MatTableResponse<LeaveLogModel>> PagedListByEmployeeIdAsync(LeaveLogFilterModel model, int employeeId);
        Task<MatTableResponse<LeaveLogModel>> GetPendingLeavesPagedListAsync(LeaveLogFilterModel model);
        Task<List<LeaveLog>> GetByRuleIdAsync(int ruleId);
        Task<bool> IsExistsAsync(int employeeId, DateTime startDate, DateTime endDate, int id);
        Task<List<LeaveLog>> GetByEmployeeIdAsync(int employeeId);
        Task<MatTableResponse<LeaveLogModel>> GetPagedListAsync(LeaveLogFilterModel model);
        Task<MatTableResponse<LeaveLogModel>> GetReporteePagedListAsync(LeaveLogFilterModel model, int userId);
        Task<LeaveLog> FindAsync(int id);
        Task<LeaveLog> PreviousLeaveLogAsync(DateTime date, int employeeId, int ruleId);
        Task<LeaveLog> GetPreviousLeaveLogDatesAsync(DateTime date, int employeeId, int ruleId);
        void Update(LeaveLog entity);
        void Update(List<LeaveLog> entity);
        Task<decimal> GetMonthlyLeaveLogAsync(int employeeId, int ruleId, DateTime start);
        Task<List<LeaveLog>> GetByRuleIdEmployeeId (int ruleId , int employeeId);

        Task<List<LeaveLogModel>> GetLeaveLog(DateTime startDate, DateTime endDate);
    }
}
