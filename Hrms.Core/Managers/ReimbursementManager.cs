using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Reimbursement;
using Hrms.Core.Utilities;
using Microsoft.Extensions.Logging;

namespace Hrms.Core.Managers
{
    public class ReimbursementManager : IReimbursementManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReimbursementRepository _reimbursementRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public ReimbursementManager(IReimbursementRepository reimbursementRepository,
            IUnitOfWork unitOfWork,
            IDocumentRepository documentRepository,
            ILogger<EmployeeManager> logger,
            IEmployeeRepository employeeRepository)
        {
            _reimbursementRepository = reimbursementRepository;
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public async Task AddAsync(ReimbursementModel model, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);

                var entity = new Reimbursement()
                {
                    Description = model.Description,
                    Amount = model.Amount,
                    Date = model.Date,
                    CreatedOn = Utility.GetDateTime(),
                    Status = Constants.RecordStatus.Pending,
                    EffectiveFrom = Utility.GetDateTime(),
                    CreatedById = userId,
                    EmployeeId = employeeId,
                };
                await _reimbursementRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                if (model.DocumentDetails != null)
                {
                    var document = new Document()
                    {
                        IdentificationId = entity.Id,
                        Name = model.DocumentDetails.Name,
                        Key = model.DocumentDetails.Key,
                        DocumentType = Constants.DocumentType.PaymentReceipt,
                        CreatedById = userId,
                        CreatedOn = Utility.GetDateTime(),
                        EffectiveFrom = Utility.GetDateTime(),
                        Status = Constants.RecordStatus.Active,
                    };
                    await _documentRepository.AddAsync(document);
                    await _unitOfWork.SaveChangesAsync();
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Reimbursement");
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<MatTableResponse<ReimbursementModel>> GetListAsync(MatDataTableRequest model)
        {
            return await _reimbursementRepository.GetListAsync(model);
        }
        public async Task<MatTableResponse<ReimbursementModel>> GetPendingListAsync(MatDataTableRequest model)
        {
            return await _reimbursementRepository.GetPendingListAsync(model);
        }

        public async Task<MatTableResponse<ReimbursementModel>> GetByEmployeeIdAsync(int id, ReimbursementFilterModel model)
        {
            return await _reimbursementRepository.GetByEmployeeIdAsync(id, model);
        }

        public async Task ChangeStatusAsync(ReimbursementChangeStatusModel model, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var entity = await _reimbursementRepository.FindAsync(model.Id);

                entity.Status = model.Status;
                entity.UpdatedById = userId;
                entity.UpdatedOn = Utility.GetDateTime();

                if (model.Status == Constants.RecordStatus.Approved)
                {
                    entity.Remark = model.Remark;
                    entity.PaymentDate = model.PaymentDate;
                }

                var doc = await _documentRepository.GetAsync(model.Id, Constants.DocumentType.PaymentReceipt);

                if (model.Status == Constants.RecordStatus.Rejected)
                {
                    entity.Remark = model.Remark;
                    doc.Status = Constants.RecordStatus.Rejected;
                }
                else
                {
                    entity.EffectiveTo = Utility.GetDateTime();
                    doc.Status = Constants.RecordStatus.Deleted;
                }

                _reimbursementRepository.Update(entity); 
                _documentRepository.Update(doc);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch(Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<MatTableResponse<ReimbursementModel>> GetPageListAsync(int id, MatDataTableRequest model)
        {
            var employee = await _employeeRepository.GetByUserIdAsync(id);
            return await _reimbursementRepository.GetPageListAsync(employee.Id, model);
        }

        public async Task<ReimbursementModel> GetByIdAsync(int id)
        {
            return await _reimbursementRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ReimbursementModel model, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var entity = await _reimbursementRepository.FindAsync(model.Id);
                var doc = await _documentRepository.GetAsync(model.Id, Constants.DocumentType.PaymentReceipt);

                entity.Description = model.Description;
                entity.Amount = model.Amount;
                entity.Date = model.Date;
                entity.UpdatedById = userId;
                entity.UpdatedOn = Utility.GetDateTime();

                if (doc != null)
                {
                    doc.Name = model.DocumentDetails.Name;
                    doc.Key = model.DocumentDetails.Key;
                    doc.UpdatedById = userId;
                    doc.UpdatedOn = Utility.GetDateTime();

                    _reimbursementRepository.Update(entity);
                    await _unitOfWork.SaveChangesAsync();
                }

                _documentRepository.Update(doc);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Change Status Reimbursement.");
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

    }
}
