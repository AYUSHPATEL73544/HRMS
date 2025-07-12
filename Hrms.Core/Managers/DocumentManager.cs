using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Utilities;
using Hrms.Core.Entities;
using Hrms.Core.Models;

namespace Hrms.Core.Managers
{
    public class DocumentManager: IDocumentManager
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DocumentManager(IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
        {
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FileDetailModel> GetAsync(int candidateId)
        {
            return await _documentRepository.GetAsync(candidateId);
        }

        public async Task AddImageAsync(FileDetailModel model, int userId)
        {

            var imageDocument = new Document
            {
                IdentificationId = userId,
                Name = model.Name,
                Key = model.Key,
                DocumentType = Constants.DocumentType.ProfileImage,
                CreatedById = userId,
                CreatedOn = Utility.GetDateTime(),
                EffectiveFrom = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
            };
            await _documentRepository.AddAsync(imageDocument);

            await _unitOfWork.SaveChangesAsync();

        }


        public async Task UpdateImageAsync(FileDetailModel model, int userId)
        {
                var existingImage = await _documentRepository.GetAsync(userId, Constants.DocumentType.ProfileImage);
                if (existingImage != null)
                {
                    existingImage.Name = model.Name;
                    existingImage.IdentificationId = userId;
                    existingImage.Key = model.Key;
                    existingImage.DocumentType = Constants.DocumentType.ProfileImage;
                    existingImage.CreatedById = userId;
                    existingImage.CreatedOn = Utility.GetDateTime();
                    existingImage.EffectiveFrom = Utility.GetDateTime();
                    existingImage.Status = Constants.RecordStatus.Active;

                    _documentRepository.Update(existingImage);
                }
                await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ImageDetailModel> GetProfileImageByUserIdAsync(int userId)
        {
            return await _documentRepository.GetProfileImageByUserIdAsync(userId);
        }

        public async Task DeleteAsync(int id)
        {
            var existingImage = await _documentRepository.FindAsync(id);
            if(existingImage != null)
            {
                _documentRepository.Delete(existingImage);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
