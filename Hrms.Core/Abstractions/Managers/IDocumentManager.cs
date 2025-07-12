using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;


namespace Hrms.Core.Abstractions.Managers
{
    public interface IDocumentManager
    {
        Task<FileDetailModel> GetAsync(int candidateId);
        Task AddImageAsync(FileDetailModel model, int userId);
        Task UpdateImageAsync(FileDetailModel model, int userId);
        Task<ImageDetailModel> GetProfileImageByUserIdAsync(int userId);
        Task DeleteAsync(int id);
    }
}
