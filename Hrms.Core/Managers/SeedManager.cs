using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace Hrms.Core.Managers
{
    public class SeedManager : ISeedManager
    {
        private readonly ILogger _logger;
        private readonly ISeedRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SeedManager(ILogger<SeedManager> logger,
            ISeedRepository repository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task InitializeAsync()
        {
            await _repository.SeedRolesAsync();
            await _repository.SeedAdminAsync();

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _repository.SeedCountryAsync();
                await _repository.SeedCompanyAsync();
                await _repository.SeedRelationshipAsync();
                await _repository.SeedQualificationTypeAsync();
                await _repository.SeedCourseTypeAsync();
                await _repository.SeedAssetTypeAsync();
                await _repository.SeedSkillsAsync();

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();

                _logger.LogError(ex, "Failed to seed data");
            }
        }
    }
}
