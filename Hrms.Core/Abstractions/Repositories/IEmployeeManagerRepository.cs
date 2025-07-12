using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IEmployeeManagerRepository
    {
        Task AddAsync(Team entity);
        Task<List<TeamModel>> GetByEmployeeIdAsync(int employeeId);
        Task<bool> IsManagerAssignedAsync(int employeeId, int managerId);
        Task<TeamModel> GetByIdAsync(int id);
        Task<List<TeamReportessModel>> GetByManagerIdAsync(int id);
        Task<List<int>> GetEmployeeIdByManagerId(int id);
        Task<Team> FindAsync(int id);
        void Update(Team entity);
    }
}
