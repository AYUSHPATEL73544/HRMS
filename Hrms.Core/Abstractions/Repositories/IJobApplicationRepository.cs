using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Entities;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IJobApplicationRepository
    {
     	Task AddAsync(JobApplication candidate);
        Task<MatTableResponse<ApplicantModel>> GetListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ShortlistCandidateModel>> GetShortlistPageListAsync(MatDataTableRequest model);
        Task<MatTableResponse<ApplicantModel>> GetCandidateListAsync(MatDataTableRequest model, int employeeId);
        void Remove(IEnumerable<ApplicantsSkill> entities);
        Task<ApplicantModel> GetDetailsAsync(int id);
        Task<bool> IsExistsAsync(string email, string number);
        Task<bool> IsExistsAsync(int id, string email, string number);
        Task<JobApplication> FindAsync(int id);
        Task<JobApplication> GetByIdAsync(int id);
        void Update(JobApplication entity);
        Task <ApplicantModel> GetAsync(int id);
    }
}
