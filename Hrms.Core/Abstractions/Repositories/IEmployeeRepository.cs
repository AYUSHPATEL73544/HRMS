using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IEmployeeRepository
    {
        Task<bool> IsCodeExistAsync(string code);
        Task AddAsync(Employee entity);
        Task<EmployeeModel> GetAsync(int id);
        Task<List<EmployeeModel>> GetByDepartmentId(int departmentId);
        Task<List<EmployeeModel>> GetByDesignationId(int designationId);

        Task<EmployeeModel> GetByUserIdAsync(int userId);
        Task<int> GetIdByUserIdAsync(int userId);
        Task<string> GetNameByIdAsync(int id);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<List<SelectListItemModel>> GetManagerListAsync(int userId);
        Task<int> GetUserIdAsync(int id);
        Task<IEnumerable<SelectListItemModel>> GetEmployeeListItemByLeaveRuleIdAsync(int leaveRuleId);
        Task<IEnumerable<SelectListItemModel>> GetEmployeeListByAttendanceRuleIdAsync(int attendanceRuleId);
        Task<List<EmployeeModel>> GetListAsync();
        Task<List<EmployeeModel>> GetActiveEmployeeListAsync();
        Task<MatTableResponse<EmployeeModel>> GetPageListAsync(MatDataTableRequest model, int employeeStatus);
        Task<MatTableResponse<EmployeeModel>> GetInactiveEmployeeListAsync(MatDataTableRequest model);
        Task<Employee> FindAsync(int id);
        Task<int> LastEmployeeAsync();
        Task<List<CompanyEventsModel>> GetCalendarEventAsync(int year, int month);
        Task<List<CompanyEventsModel>> GetLeaveLogCalendarEventAsync(int year, int month, int employeeId);
        void Update(Employee entity);
    }
}
