using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Models.Reimbursement;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class ReimbursementRepository : IReimbursementRepository
    {
        private readonly DataContext _dataContext;

        public ReimbursementRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Reimbursement entity)
        {
            await _dataContext.Reimbursements.AddAsync(entity);
        }

        public async Task<MatTableResponse<ReimbursementModel>> GetListAsync(MatDataTableRequest model)
        {
            var recordsToSkip = model.RecordsToSkip();
            var sortExpression = model.SortExpression();

            var linqStmt = from r in _dataContext.Reimbursements
                           join e in _dataContext.Employees on r.EmployeeId equals e.Id
                           join d in _dataContext.Documents on r.Id equals d.IdentificationId
                           where r.Status != Constants.RecordStatus.Pending
                           && r.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%"))
                           select new ReimbursementModel
                           {
                               Id = r.Id,
                               EmployeeId = e.Id,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               Description = r.Description,
                               PaymentDate = r.PaymentDate != null ? r.PaymentDate : null,
                               Amount = r.Amount,
                               Date = r.Date,
                               CreatedOn = r.CreatedOn,
                               Status = r.Status,
                               DocumentDetails = new FileDetailModel
                               {
                                   Key = d.Key,
                                   DocumentType = d.DocumentType
                               },
                           };

            var response = new MatTableResponse<ReimbursementModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                                .OrderBy(sortExpression)
                                .Skip(recordsToSkip)
                                .Take(model.PageSize)
                                .ToListAsync()
            };

            return response;
        }
        public async Task<MatTableResponse<ReimbursementModel>> GetPendingListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordToSkip = model.RecordsToSkip();

            var linqStmt = from rb in _dataContext.Reimbursements
                           join e in _dataContext.Employees on rb.EmployeeId equals e.Id
                           join doc in _dataContext.Documents on rb.Id equals doc.IdentificationId
                           where rb.Status == Constants.RecordStatus.Pending &&
                           (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%"))
                           select new ReimbursementModel
                           {
                               Id = rb.Id,
                               EmployeeId = e.Id,
                               EmployeeName = e.FirstName + " " + e.LastName,
                               Description = rb.Description,
                               Amount = rb.Amount,
                               Date = rb.Date,
                               CreatedOn = rb.CreatedOn,
                               Status = rb.Status,
                               DocumentDetails = new FileDetailModel
                               {
                                   Key = doc.Key,
                                   DocumentType = doc.DocumentType
                               }
                           };

            var response = new MatTableResponse<ReimbursementModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                        .OrderBy(sortExpression)
                        .Skip(recordToSkip)
                        .Take(model.PageSize).ToListAsync()
            };

            return response;
        }

        public void Update(Reimbursement entity)
        {
            _dataContext.Reimbursements.Update(entity);
        }

        public async Task<Reimbursement> FindAsync(int id)
        {
            return await _dataContext.Reimbursements.FindAsync(id);
        }

        public async Task<MatTableResponse<ReimbursementModel>> GetByEmployeeIdAsync(int id, ReimbursementFilterModel model)
        {
            var sortExpression = model.SortExpression();

            var recordToSkip = model.RecordsToSkip();

            var linqStmt = from rb in _dataContext.Reimbursements
                           join emp in _dataContext.Employees on rb.EmployeeId equals emp.Id
                           join doc in _dataContext.Documents on rb.Id equals doc.IdentificationId
                           where rb.EmployeeId == id
                           && rb.Status != Constants.RecordStatus.Deleted
                           && (model.StartDate == null || rb.Date.Date >= model.StartDate)
                           && (model.EndDate == null || rb.Date.Date <= model.EndDate)
                           select new ReimbursementModel
                           {
                               Id = rb.Id,
                               EmployeeName = emp.FirstName + " " + emp.LastName,
                               Description = rb.Description,
                               Amount = rb.Amount,
                               Date = rb.Date,
                               CreatedOn = rb.CreatedOn,
                               Status = rb.Status,
                               PaymentDate = rb.PaymentDate != null ? rb.PaymentDate : null,
                               DocumentDetails = new FileDetailModel
                               {
                                   Key = doc.Key,
                                   DocumentType = doc.DocumentType
                               }
                           };

            var response = new MatTableResponse<ReimbursementModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                        .OrderBy(sortExpression)
                        .Skip(recordToSkip)
                        .Take(model.PageSize).ToListAsync()
            };

            return response;
        }

        public async Task<MatTableResponse<ReimbursementModel>> GetPageListAsync(int id, MatDataTableRequest model)
        {
            var sortExression = model.SortExpression();

            var recordToSkip = model.RecordsToSkip();

            var linqStmt = from reimbursement in _dataContext.Reimbursements
                           join employee in _dataContext.Employees on reimbursement.EmployeeId equals employee.Id
                           join document in _dataContext.Documents on reimbursement.Id equals document.IdentificationId
                           where employee.Id == id
                           && reimbursement.Status != Constants.RecordStatus.Deleted
                           && (model == null || EF.Functions.Like(reimbursement.Description, "%" + model.FilterKey + "%"))
                           select new ReimbursementModel
                           {
                               EmployeeName = employee.FirstName + " " + employee.LastName,
                               Id = reimbursement.Id,
                               Description = reimbursement.Description,
                               Amount = reimbursement.Amount,
                               Date = reimbursement.Date,
                               PaymentDate = reimbursement.PaymentDate != null ? reimbursement.PaymentDate : null,
                               CreatedOn = reimbursement.CreatedOn,
                               Status = reimbursement.Status,
                               DocumentDetails = new FileDetailModel
                               {
                                   Key = document.Key,
                                   DocumentType = document.DocumentType
                               }
                           };

            var response = new MatTableResponse<ReimbursementModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                        .OrderBy(sortExression)
                        .Skip(recordToSkip)
                        .Take(model.PageSize).ToListAsync()
            };

            return response;
        }

        public async Task<ReimbursementModel> GetByIdAsync(int id)
        {
            return await (from rb in _dataContext.Reimbursements
                          join doc in _dataContext.Documents on rb.Id equals doc.IdentificationId
                          where rb.Id == id
                          select new ReimbursementModel
                          {
                              Id = rb.Id,
                              Description = rb.Description,
                              Amount = rb.Amount,
                              Date = rb.Date,
                              CreatedOn = rb.CreatedOn,
                              DocumentDetails = new FileDetailModel
                              {
                                  Name = doc.Name,
                                  Key = doc.Key,
                                  DocumentType = doc.DocumentType
                              }
                          }).SingleAsync();
        }

    }
}
