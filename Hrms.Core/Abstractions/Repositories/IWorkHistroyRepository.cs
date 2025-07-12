using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IWorkHistroyRepository
    {
        Task AddAsync(WorkHistory entity);
        Task<List<WorkHistoryModel>> GetAsync(int employeeId);
        Task<WorkHistoryModel> GetByIdAsync(int id);
        Task<WorkHistory> GetByPreviousWorkHistoryAsync(int? departmentId, int? designationId, int employeeId);
        Task<WorkHistory> FindAsync(int id);
        void Update(WorkHistory entity);
    }
}
