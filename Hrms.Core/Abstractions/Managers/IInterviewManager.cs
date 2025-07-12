using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;

namespace Hrms.Core.Abstractions.Managers
{
    public interface IInterviewManager
    {
        Task AddAsync(InterviewModel model, int userId);
        Task<MatTableResponse<InterviewModel>> GetPagedListAsync(MatDataTableRequest model, int userId);
        Task<InterviewModel> GetDetailAsync(int id);
        Task<InterviewModel> GetAsync(int id);
        Task<List<InterviewModel>> GetListByCandidateIdAsync(int candidateId);
        Task UpdateAsync(InterviewModel model, int userId);
    }
}
