using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IFamilyRepository
    {
        Task AddAsync(Family entity);
        Task<List<FamilyModel>> GetAsync();
        Task<FamilyModel> GetByIdAsync(int id);
        Task<MatTableResponse<FamilyModel>> GetPagedListAsync(MatDataTableRequest model, int employeeId);
        Task<Family> FindAsync(int id);
        void Update(Family entity);
    }
}
