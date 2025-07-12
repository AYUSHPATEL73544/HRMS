using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;


namespace Hrms.Core.Abstractions.Managers
{
    public interface ILeaveManager
    {
        Task AddAsync(LeaveModel model);
        Task<LeaveModel> GetAsync(int id);
        Task<List<LeaveModel>> GetDetailAsync();
        Task<List<LeaveModel>> GetLeaveBalanceListAsync();
        Task<List<LeaveModel>> GetListAsync(int userId); 
        Task<MatTableResponse<LeaveModel>> GetPagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<LeaveModel>> GetAssignRuleListAsync(MatDataTableRequest model); 
        Task<MatTableResponse<LeaveModel>> GetInactiveAssignListAsync(MatDataTableRequest model);
        Task<Leave> GetByRuleId(int ruleId, int userId);
        Task UpdateAsync(LeaveModel model);
        Task DeleteAsync(int employeeId, int ruleId);
    }
}
