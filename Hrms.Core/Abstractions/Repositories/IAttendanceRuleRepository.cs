using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IAttendanceRuleRepository
    {
        Task AddAsync(AttendanceRule entity);
        Task<MatTableResponse<AttendanceRuleModel>> GetPagedListAsync(MatDataTableRequest model);
        Task<MatTableResponse<AttendanceRuleModel>> GetPagedListAsync(MatDataTableRequest model, int employeeId);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<MatTableResponse<AttendanceRuleListItemModel>> GetPagedResultAsync(MatDataTableRequest model);
        Task<AttendanceRuleModel> GetAsync(int id);
        Task<AttendanceRule> FindAsync(int id);
        void Update(AttendanceRule entity);
        Task<AttendanceRuleModel> GetByYearAsync(int year);
        Task<bool> IsExistsAsync(int year);
    }
}
