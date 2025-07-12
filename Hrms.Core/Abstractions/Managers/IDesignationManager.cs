using Hrms.Core.Models;
using Hrms.Core.Models.Company;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IDesignationManager
    {
        Task<DesignationModel> GetDetailAsync(int id);
        Task<List<SelectListItemModel>> GetSelectListItemAsync();
        Task AddAsync(DesignationModel model);
        Task UpdateAsync(DesignationModel model);
        Task<MatTableResponse<DesignationModel>> GetPageListAsync(MatDataTableRequest model);
        Task DeleteAsync(int id);
        Task<bool> IsDesignationExistAsync(string designation);
    }
}
