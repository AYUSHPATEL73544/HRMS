using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;


namespace Hrms.Core.Abstractions.Managers
{
    public interface ILeaveLogManager
    {
        Task<bool> IsExistsAsync(int userId, DateTime startDate, DateTime endDate, int id = 0);
        Task AddAsync(LeaveLogModel model, int userId);
        Task<LeaveLogModel> GetAsync(int id);
        Task<MatTableResponse<LeaveLogModel>> PagedListByUserIdAsync(LeaveLogFilterModel model, int userId);
        Task<MatTableResponse<LeaveLogModel>> PagedListByEmployeeIdAsync(LeaveLogFilterModel model, int employeeId);
        Task<MatTableResponse<LeaveLogModel>> GetPendingLeavesPagedListAsync(LeaveLogFilterModel model);
        Task<MatTableResponse<LeaveLogModel>> GetPagedListAsync(LeaveLogFilterModel model);
        Task<MatTableResponse<LeaveLogModel>> GetReporteePagedListAsync(LeaveLogFilterModel model, int userId);
        Task<int> GetTotalLeaveCountAsync(LeaveLogModel model, int userId);

        Task UpdateAsync(LeaveLogModel model);
        Task ChangeStatusAsync(LeaveLogChangeStatusModel model);
        
        Task DeleteAsync(int id);

        Task<List<LeaveLogModel>> GetLeaveLog(DateTime startDate,DateTime endDate);
    }
}
