using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;


namespace Hrms.Infrastructure.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        public readonly DataContext _dataContext;

        public DocumentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task AddAsync(Document document)
        {
            await _dataContext.AddAsync(document);
        }

        public void Update(Document entity)
        {
            _dataContext.Documents.Update(entity);
        }
        public async Task<FileDetailModel> GetAsync(int candidateId)
        {
            return await _dataContext.Documents
                        .Where(x => x.IdentificationId == candidateId
                            && x.Status == Constants.RecordStatus.Active)
                        .Select(x => new FileDetailModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Key = x.Key,
                        }).SingleOrDefaultAsync();
        }

        public async Task<Document> GetAsync(int identificationId, Constants.DocumentType documentType)
        {
            return await _dataContext.Documents
                                     .Where(x => x.IdentificationId == identificationId
                                                  && x.DocumentType == documentType
                                                  && x.Status != Constants.RecordStatus.Deleted)
                                     .SingleAsync();
         }
                                     
        public async Task<Document> FindAsync(int id)
        {
            return await _dataContext.Documents.FindAsync(id);
        }

        public async Task<ImageDetailModel> GetProfileImageByUserIdAsync(int userId)
        {
            var res = await (from d in _dataContext.Documents
                          where d.IdentificationId == userId
                          && d.DocumentType == Constants.DocumentType.ProfileImage
                          select new ImageDetailModel
                          {
                              Name = d.Name,
                              Key = d.Key,
                          }).SingleOrDefaultAsync();

            return res;
        }



        public void Delete(Document entity)
        {
              _dataContext.Remove(entity);
        }
    }
}
