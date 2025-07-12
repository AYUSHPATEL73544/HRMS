using Hrms.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IWorkHistoryManager
    {
        Task<List<WorkHistoryModel>> GetAsync(int employeeId);
        Task UpdateAsync(WorkHistoryModel model);
        Task<WorkHistoryModel> GetByIdAsync(int id);
        Task<List<WorkHistoryModel>> GetByUserIdAsync(int userId);
    }
}
