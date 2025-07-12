using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface ILeaveRuleRepository
    {
        Task AddAsync(LeaveRule entity);
        Task<LeaveRuleModel> GetAsync(int id);
        Task<MatTableResponse<LeaveRuleModel>> GetListAsync(MatDataTableRequest model, int employeeId);
        Task<List<LeaveRuleModel>> GetListByEmployeeIdAsync(int employeeId);
        Task<MatTableResponse<LeaveRuleModel>> GetPageListAsync(MatDataTableRequest model);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<List<int>> GetRuleIdListAsync();
        Task<LeaveRule> FindAsync(int id);
        void Update(LeaveRule entity);
    }
}
