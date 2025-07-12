using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Reimbursement;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IReimbursementManager
    {
        Task AddAsync(ReimbursementModel model,int userId);
        Task<MatTableResponse<ReimbursementModel>> GetListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ReimbursementModel>> GetPendingListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ReimbursementModel>> GetByEmployeeIdAsync(int id, ReimbursementFilterModel model);
        Task ChangeStatusAsync(ReimbursementChangeStatusModel model, int userId);
        Task<MatTableResponse<ReimbursementModel>> GetPageListAsync(int id, MatDataTableRequest model);
        Task<ReimbursementModel> GetByIdAsync(int id);
        Task UpdateAsync(ReimbursementModel model, int userId); 
    }
}
