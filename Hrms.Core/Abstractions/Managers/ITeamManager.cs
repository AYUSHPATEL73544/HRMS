using Hrms.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Managers
{
    public interface ITeamManager
    {
        Task AddAsync(TeamModel model);
        Task<List<TeamModel>> GetByEmployeeIdAsync(int employeeId);
        Task<bool> IsManagerAssignedAsync(int employeeId, int managerId);
        Task UpdateAsync(TeamModel model);
        Task<TeamModel> GetByIdAsync(int id);
        Task<List<TeamModel>> GetByUserIdAsync(int userId);
        Task<List<TeamReportessModel>> GetReportessListAsync(int userId);
        Task<List<TeamReportessModel>> GetByManagerIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
