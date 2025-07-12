using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Reimbursement;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IReimbursementRepository
    {
        Task AddAsync(Reimbursement entity);
        Task<MatTableResponse<ReimbursementModel>> GetListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ReimbursementModel>> GetPendingListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ReimbursementModel>> GetByEmployeeIdAsync(int id, ReimbursementFilterModel model);
        Task<Reimbursement> FindAsync(int id); 
        void Update(Reimbursement entity);
        Task<MatTableResponse<ReimbursementModel>> GetPageListAsync(int id, MatDataTableRequest model);
        Task<ReimbursementModel> GetByIdAsync(int id); 
    }
}
