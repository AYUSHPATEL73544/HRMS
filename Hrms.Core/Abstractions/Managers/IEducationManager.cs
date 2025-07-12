using Hrms.Core.Models;
using Hrms.Core.Models.Employee;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IEducationManager
    {
        Task AddAsync(EducationModel model, int userId);
        Task<EducationModel> GetByIdAsync(int id);
        Task<MatTableResponse<EducationModel>> GetPageListAsync(MatDataTableRequest model, int userId);
        Task<MatTableResponse<EducationModel>> GetPageListByEmployeeIdAsync(MatDataTableRequest model, int employeeId);
        Task UpdateAsync(EducationModel model);
        Task DeleteAsync(int id);
    }
}
