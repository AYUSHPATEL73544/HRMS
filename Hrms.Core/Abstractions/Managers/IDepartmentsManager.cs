using Hrms.Core.Models;
using Hrms.Core.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IDepartmentsManager
    {
        Task AddAsync(DepartmentModel model);
        Task<DepartmentModel> GetDetailAsync(int id);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task<MatTableResponse<DepartmentModel>> GetPagedListAsync(MatDataTableRequest model);
        Task UpdateAsync(DepartmentModel model);
        Task DeleteAsync(int id);
        Task<bool> IsExistsAsync(string departmentName);
    }
}
