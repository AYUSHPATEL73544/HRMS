

using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Models.Leave;
namespace Hrms.Core.Abstractions.Managers
{
    public interface IEmployeeManager
    {
        Task<bool> IsCodeExistAsync(string code);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsCodeExistAsync( int id , string code);
        Task<bool> IsEmailExistsAsync(int id , string email);
        Task AddAsync(EmployeeModel model);
        Task<EmployeeModel> GetAsync(int id);
        Task<string> GetNameByIdAsync(int id);
        Task<EmployeeModel> GetByUserIdAsync(int userId);
        Task<List<EmployeeModel>> GetByDepartmentId(int departmentId);
        Task<List<EmployeeModel>> GetByDesignationId(int designationId);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<List<SelectListItemModel>> GetManagerListAsync(int userId);
        Task<IEnumerable<SelectListItemModel>> GetEmployeeListItemByLeaveRuleIdAsync(int leaveRuleId);
        Task<IEnumerable<SelectListItemModel>> GetEmployeeListByAttendanceRuleIdAsync(int attendanceRuleId);
        Task<List<EmployeeModel>> GetListAsync();
        Task<int> GetUserIdAsync(int id);
        Task<MatTableResponse<EmployeeModel>> GetPageListAsync(MatDataTableRequest model , int employeeStatus);
        Task<MatTableResponse<EmployeeModel>> GetInactiveEmployeeListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ApplicantModel>> GetCandidateListAsync(MatDataTableRequest model, int employeeId);
        Task<List<CompanyEventsModel>> GetCalendarEventAsync(int year, int month);
        Task<List<CompanyEventsModel>> GetLeaveLogCalendarEventAsync(int year, int month, int employeeId);
        Task<List<CompanyEventsModel>> GetEmployeeLeaveCalendarEventAsync(int year, int month, int userId);
        Task UpdateAsync(EmployeeModel model, int userId);
        Task DeleteAsync(int id);
    }
}
