using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IJobApplicationManager
    {
        Task AddAsync(ApplicantModel model, int userId);
        Task<MatTableResponse<ApplicantModel>> GetListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ShortlistCandidateModel>> GetShortlistPageListAsync(MatDataTableRequest model);
        Task<bool> IsExistsAsync(string email, string phoneNumber);
        Task<bool> IsExistsAsync(int id, string email, string phoneNumber);
        Task<ApplicantModel> GetDetailsAsync(int id);
        Task DeleteAsync(int id);
        Task<ApplicantModel> GetAsync(int id);
        Task ShortlistAsync(int id, int userId);
        Task HireAsync(HireModel model, int userId);
        Task UpdateAsync (ApplicantModel model , int userId);
        Task ChangeStatusAsync(ApplicantChangeStatusModel model, int userId);
    }
}
