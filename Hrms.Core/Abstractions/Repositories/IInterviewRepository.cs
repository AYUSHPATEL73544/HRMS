using Hrms.Core.Entities;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Models;


namespace Hrms.Core.Abstractions.Repositories
{
    public interface IInterviewRepository
    {
        Task AddAsync(Interview entity);
        Task<MatTableResponse<InterviewModel>> GetPagedListAsync(MatDataTableRequest model, int userId);
        Task<InterviewModel> GetAsync(int id);
        Task<List<InterviewModel>> GetListByCandidateIdAsync(int candidateId);
        Task<InterviewModel> GetDetailAsync(int id);
        void Update(Interview entity);
        Task<Interview> FindAsync(int id);
    }
}
