using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface ILeaveRepository
    {
        Task AddAsync(Leave entity);
        Task<LeaveModel> GetAsync(int id);
        Task<Leave> GetByRuleIdAsync(int employeeId, int ruleId);
         Task<List<Leave>> GetByRuleIdAsync(int ruleId);
        Task<List<LeaveModel>> GetListAsync();
        Task<List<LeaveModel>> GetListAsync(int employeeId); 
        Task<MatTableResponse<LeaveModel>> GetPagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<LeaveModel>> GetAssignRuleListAsync(MatDataTableRequest model); 
         Task<MatTableResponse<LeaveModel>> GetInactiveAssignListAsync(MatDataTableRequest model);
        Task<List<Leave>> GetListByEmployeeIdAsync(int employeeId);
        Task<Leave> FindAsync(int id);
        void Update(Leave entity);
        decimal CalculateLeavesToCredit(DateTime joiningDate, int totalLeave, int leaveCredited = 0);
        decimal CalculateLossOfPayCreditLeaves();
    }
}
