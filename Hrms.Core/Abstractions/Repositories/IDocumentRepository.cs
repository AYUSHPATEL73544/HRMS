using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;

namespace Hrms.Core.Abstractions.Repositories
{
    public interface IDocumentRepository
    {
        Task AddAsync(Document document);
        void  Update(Document entity);
        Task<FileDetailModel> GetAsync(int candidateId);
        Task<Document> GetAsync(int identificationId, Constants.DocumentType documentType);
        Task<Document> FindAsync(int id);
        Task<ImageDetailModel> GetProfileImageByUserIdAsync(int userId);
        void Delete(Document entity);

    }
}
