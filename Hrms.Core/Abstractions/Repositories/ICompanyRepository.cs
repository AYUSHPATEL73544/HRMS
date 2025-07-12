using Hrms.Core.Entities;
using Hrms.Core.Models.Company;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(int id);
        Task<CompanyModel> GetAsync(int id);
        void Update(Company entity);
        Task<int> GetIdByUserIdAsync(int userId);
    }
}
