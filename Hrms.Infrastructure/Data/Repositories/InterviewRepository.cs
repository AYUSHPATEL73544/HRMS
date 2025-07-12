using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        public readonly DataContext _dataContext;

        public InterviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Interview entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<MatTableResponse<InterviewModel>> GetPagedListAsync(MatDataTableRequest model, int userId)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from i in _dataContext.Interviews
                           join c in _dataContext.JobApplications on i.CandidateId equals c.Id
                           where i.InterviewerId == userId
                           && i.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                               || EF.Functions.Like(c.FirstName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(c.LastName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(c.Email, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(c.Phone, "%" + model.FilterKey + "%"))
                           select new InterviewModel
                           {
                               Id = i.Id,
                               CandidateId = i.CandidateId,
                               ScheduleDate = i.ScheduleDate,
                               ScheduleTime = i.ScheduleTime,
                               InterviewerId = i.InterviewerId,
                               Phone = c.Phone,
                               Email = c.Email,
                               LegalName = c.FirstName + " " + c.LastName,
                               EligibleForNextRound = i.EligibleForNextRound,
                               Status = i.Status,
                               DocumentDetails = _dataContext.Documents
                                                .Where(x => x.IdentificationId == c.Id
                                                 && x.DocumentType == Constants.DocumentType.Cv
                                                 && x.Status == Constants.RecordStatus.Active)
                                                .Select(x => new FileDetailModel
                                                  {
                                                  Id = x.Id,
                                                  Name = x.Name,
                                                  Key = x.Key,
                                                 }).FirstOrDefault(),
                           };

            var response = new MatTableResponse<InterviewModel>
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

        public async Task<InterviewModel> GetAsync(int id)
        {
            return await (from c in _dataContext.JobApplications
                          join i in _dataContext.Interviews on c.Id equals i.CandidateId
                          join e in _dataContext.Employees on i.InterviewerId equals e.Id
                          where i.Id == id
                          select new InterviewModel
                          {
                              LegalName = c.FirstName + " " + c.LastName,
                              Rating = i.Rating,
                              EligibleForNextRound = i.EligibleForNextRound,
                              Remark = i.Remark,
                              InterviewerName = e.FirstName + " " + e.LastName,
                              InterviewDate = i.InterviewDate,
                          }).SingleAsync();
        }

        public async Task<List<InterviewModel>> GetListByCandidateIdAsync(int candidateId)
        {
            return await (from i in _dataContext.Interviews
                          join c in _dataContext.JobApplications on i.CandidateId equals c.Id
                          join u in _dataContext.Users on i.InterviewerId equals u.Id
                          where i.CandidateId == candidateId
                          select new InterviewModel
                          {
                              Id = i.Id,
                              InterviewerId = i.InterviewerId,
                              InterviewerName = u.FirstName + " " + u.LastName,
                              LegalName = c.FirstName + " " + c.LastName,
                              Email = c.Email,
                              ScheduleDate = i.ScheduleDate,
                              ScheduleTime = i.ScheduleTime,
                              InterviewMode = i.InterviewMode,
                              InterviewType = i.InterviewType,
                              InterviewDate = i.InterviewDate,
                              EligibleForNextRound = i.EligibleForNextRound,
                              Remark = i.Remark,
                              Status = i.Status,
                              Rating = i.Rating,
                          }).ToListAsync();
        }

        public async Task<InterviewModel> GetDetailAsync(int id)
        {
            return await _dataContext.Interviews
                        .Where(x => x.Id == id)
                        .Select(x => new InterviewModel
                        {
                            Id = x.Id,
                            InterviewMode = x.InterviewMode,
                            InterviewType = x.InterviewType,
                            ScheduleDate = x.ScheduleDate,
                            ScheduleTime = x.ScheduleTime,
                            InterviewerId = x.InterviewerId,
                            InterviewDate = x.InterviewDate,
                            Rating = x.Rating,
                            Remark = x.Remark,
                            EligibleForNextRound = x.EligibleForNextRound,
                        }).SingleAsync();
        }

        public async Task<Interview> FindAsync(int id)
        {
            return await _dataContext.Interviews.FindAsync(id);
        }

        public void Update(Interview entity)
        {
            _dataContext.Interviews.Update(entity);
        }
    }
}
