using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Company;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IDepartmentRepository
    {
        Task AddAsync(Department entity);
        Task<DepartmentModel> GetDetailAsync(int? id);
        Task<MatTableResponse<DepartmentModel>> GetPagedListAsync(MatDataTableRequest model);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<Department> FindAsync(int id);
        void Update(Department entity);
        Task<bool> IsExistsAsync(string departmentName);
    }
}
