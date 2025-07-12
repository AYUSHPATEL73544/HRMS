using Hrms.Core.Models;
using Hrms.Core.Models.Leave;


namespace Hrms.Core.Abstractions.Managers
{
    public interface ILeaveRuleManager
    {
        Task AddAsync(LeaveRuleModel model);
        Task<LeaveRuleModel> GetAsync(int id);
        Task<MatTableResponse<LeaveRuleModel>> GetListAsync(MatDataTableRequest model, int userId);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<MatTableResponse<LeaveRuleModel>> GetPageListAsync(MatDataTableRequest model);
        Task UpdateAsync(LeaveRuleModel model);
        Task DeleteAsync(int id);
    }
}
