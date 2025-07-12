using Hrms.Core.Models;
using Hrms.Core.Models.Employee;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IFamilyManager
    {
        Task AddAsync(FamilyModel model, int userId);
        Task<List<FamilyModel>> GetAsync();
        Task<FamilyModel> GetByIdAsync(int id);
        Task<MatTableResponse<FamilyModel>> GetPagedListAsync(MatDataTableRequest model, int employeeId);
        Task<MatTableResponse<FamilyModel>> GetByUserIdAsync(MatDataTableRequest model, int userId);
        Task UpdateAsync(FamilyModel model);
        Task DeleteAsync(int id);
    }
}
