using Hrms.Core.Models.Company;

namespace Hrms.Core.Abstractions.Managers
{
    public interface ICompanyManager
    {
        Task<CompanyModel> GetAsync(int id);
        Task UpdateAsync(CompanyModel model, int userId);
        Task<int?> GetIdByUserIdAsync(int userId);
    }
}
