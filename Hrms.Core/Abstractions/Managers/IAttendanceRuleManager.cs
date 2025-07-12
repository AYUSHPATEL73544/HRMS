using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IAttendanceRuleManager
    {
        Task AddAsync(AttendanceRuleModel model);
        Task<AttendanceRuleModel> GetAsync(int id);
        Task<MatTableResponse<AttendanceRuleModel>> GetPagedListAsync(MatDataTableRequest model, int userId);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task UpdateAsync(AttendanceRuleModel model);
        Task<MatTableResponse<AttendanceRuleListItemModel>> GetPagedResultAsync(MatDataTableRequest model);
        Task DeleteAsync(int id);
        Task<AttendanceRuleModel> GetByYearAsync(int year);
        Task<bool> IsExistsAsync(int year);



    }
}
