using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IEducationRepository
    {
        Task AddAsync(Education entity);
        Task<EducationModel> GetByIdAsync(int id);
        Task<MatTableResponse<EducationModel>> GetPageListAsync(MatDataTableRequest model, int employeeId);
        Task<Education> FindAsync(int id);
        void Update(Education entity);
    }
}
