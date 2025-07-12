using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Models;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Entities;
using MySql.EntityFrameworkCore.Extensions;
using System.Linq.Dynamic.Core;
using Hrms.Core.Utilities;
using Hrms.Core.Models.Assest;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly DataContext _dataContext;

        public JobApplicationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task AddAsync(JobApplication candidate)
        {
            await _dataContext.AddAsync(candidate);
        }

        public async Task<bool> IsExistsAsync(string email, string number)
        {
            return await _dataContext.JobApplications
                        .AsNoTracking()
                        .AnyAsync(x => x.Email.Equals(email)
                                && x.Phone.Equals(number)
                                && x.Status == Constants.RecordStatus.Active);
        }

        public async Task<bool> IsExistsAsync(int id, string email, string number)
        {
            return await _dataContext.JobApplications
                        .AsNoTracking()
                        .AnyAsync(x => x.Email.Equals(email)
                                && x.Phone.Equals(number)
                                && x.Id != id
                                && x.Status == Constants.RecordStatus.Active);
        }

        public async Task<MatTableResponse<ApplicantModel>> GetListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from ca in _dataContext.JobApplications
                           where ca.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                               || EF.Functions.Like(ca.FirstName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(ca.LastName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(ca.FirstName + " " + ca.LastName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(ca.Email, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(ca.Phone, "%" + model.FilterKey + "%"))
                           select new ApplicantModel
                           {
                               Id = ca.Id,
                               FirstName = ca.FirstName,
                               LastName = ca.LastName,
                               Phone = ca.Phone,
                               Email = ca.Email,
                               CreatedDate = ca.CreatedOn,
                               QualificationTypeId = ca.QualificationTypeId,
                               PassingYear = ca.PassingYear,
                               Remark = ca.Remark,
                               IsHired = ca.Hired,
                               IsPursuing = ca.Pursuing,
                               CourseName = ca.CourseName,
                               Stream = ca.Stream,
                               CourseTypeId = ca.CourseTypeId,
                               Status = ca.Status,
                               SkillIds = _dataContext.ApplicantsSkills
                                                .Where(x => x.ApplicantId == ca.Id)
                                                    .Select(x => x.SkillId).ToList(),
                               DocumentDetails = _dataContext.Documents
                                                  .Where(x => x.IdentificationId == ca.Id
                                                        && x.DocumentType == Constants.DocumentType.Cv
                                                        && x.Status == Constants.RecordStatus.Active)
                                                  .Select(x => new FileDetailModel
                                                  {
                                                      Id = x.Id,
                                                      Name = x.Name,
                                                      Key = x.Key,
                                                      DocumentType = x.DocumentType,
                                                  }).FirstOrDefault(),
                           };

            var response = new MatTableResponse<ApplicantModel>
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

        public async Task<MatTableResponse<ShortlistCandidateModel>> GetShortlistPageListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from ca in _dataContext.JobApplications
                           join i in _dataContext.Interviews on ca.Id equals i.CandidateId into interviews
                           from ii in interviews.DefaultIfEmpty()
                           where ca.Shortlisted
                           && (model.FilterKey == null
                               || EF.Functions.Like(ca.FirstName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(ca.LastName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(ca.Phone, "%" + model.FilterKey + "%"))
                           select new ShortlistCandidateModel
                           {
                               CandidateId = ca.Id,
                               FirstName = ca.FirstName,
                               LastName = ca.LastName,
                               Email = ca.Email,
                               Phone = ca.Phone,
                               ShortlistedDate = ca.ShortlistedDate,
                               ScheduleDate = ii.ScheduleDate,
                               ScheduleTime = ii.ScheduleTime,
                               LegalName = ca.FirstName + " " + ca.LastName,
                               Status = ii.Status.Equals(null) ? Constants.RecordStatus.Pending : ii.Status,
                               InterviewId = ii.Id
                           };

            var response = new MatTableResponse<ShortlistCandidateModel>
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

        public async Task<MatTableResponse<ApplicantModel>> GetCandidateListAsync(MatDataTableRequest model, int employeeId)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from c in _dataContext.JobApplications
                           join i in _dataContext.Interviews on c.Id equals i.CandidateId
                           join e in _dataContext.Employees on i.InterviewerId equals e.Id
                           where c.Status == Constants.RecordStatus.Active
                           && e.Id == employeeId
                           && (model.FilterKey == null
                             || EF.Functions.Like(c.FirstName, "%" + model.FilterKey + "%")
                             || EF.Functions.Like(c.LastName, "%" + model.FilterKey + "%")
                             || EF.Functions.Like(c.Email, "%" + model.FilterKey + "%")
                             || EF.Functions.Like(c.Phone, "%" + model.FilterKey + "%"))
                           select new ApplicantModel
                           {
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                               Phone = c.Phone,
                               Email = c.Email,
                               ScheduleDate = i.ScheduleDate,
                               InterviewStatus = i.Status
                           };

            var response = new MatTableResponse<ApplicantModel>
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

        public async Task<JobApplication> FindAsync(int id)
        {
            return await _dataContext.JobApplications.FindAsync(id);
        }

        public async Task<JobApplication> GetByIdAsync(int id)
        {
            return await _dataContext.JobApplications
                          .Include(x => x.ApplicantsSkills)
                          .Where(x => x.Id == id)
                          .Select(x => x)
                          .SingleAsync();
        }

        public async Task<ApplicantModel> GetDetailsAsync(int id)
        {
            var linq = await (from c in _dataContext.JobApplications
                              join i in _dataContext.Interviews on c.Id equals i.CandidateId into candidtes
                              from ii in candidtes.DefaultIfEmpty()
                              join e in _dataContext.Employees on ii.InterviewerId equals e.Id into emp
                              from ee in emp.DefaultIfEmpty()
                              where c.Id == id
                              && c.Status != Constants.RecordStatus.Deleted
                              select new ApplicantModel
                              {
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Email = c.Email,
                                  Phone = c.Phone,
                                  QualificationTypeId = c.QualificationTypeId,
                                  CourseTypeId = c.CourseTypeId,
                                  InterviewerName = ee.FirstName + " " + ee.LastName,
                                  Stream = c.Stream,
                                  InterviewMode = ii.InterviewMode,
                                  InterviewType = ii.InterviewType,
                                  CourseName = c.CourseName,
                                  Gender = c.Gender,
                                  MarketingChannel = c.MarketingChannel,
                                  CreatedOn = c.CreatedOn,
                                  SkillIds = _dataContext.ApplicantsSkills
                                              .Where(x => x.ApplicantId == id)
                                              .Select(x => x.SkillId).ToList(),
                              }).SingleAsync();

            linq.SkillNames = await _dataContext.Skills
                              .Where(x => linq.SkillIds.Contains(x.Id))
                              .Select(x => x.Name).ToListAsync();

            return linq;
        }

        public void Update(JobApplication entity)
        {
            _dataContext.JobApplications.Update(entity);
        }

        public void Remove(IEnumerable<ApplicantsSkill> entities)
        {
            _dataContext.ApplicantsSkills.RemoveRange(entities);
        }

        public async Task<ApplicantModel> GetAsync(int id)
        {
            var response = await (from c in _dataContext.JobApplications
                                  where c.Id == id
                                  select new ApplicantModel
                                  {
                                      Id = c.Id,
                                      FirstName = c.FirstName,
                                      LastName = c.LastName,
                                      Phone = c.Phone,
                                      Email = c.Email,
                                      QualificationTypeId = c.QualificationTypeId,
                                      CourseTypeId = c.CourseTypeId,
                                      Stream = c.Stream,
                                      ShortlistedDate = c.ShortlistedDate,
                                      IsHired = c.Hired,
                                      IsShortlisted = c.Shortlisted,
                                      CourseName = c.CourseName,
                                      Gender = c.Gender,
                                      PassingYear = c.PassingYear,
                                      IsPursuing = c.Pursuing,
                                      Status = c.Status,
                                      Remark = c.Remark,
                                      MarketingChannel = c.MarketingChannel,
                                      CreatedOn = c.CreatedOn,
                                      SkillIds = (from cs in _dataContext.ApplicantsSkills
                                                  where cs.ApplicantId == id
                                                  select cs.SkillId).ToList(),
                                  }).FirstOrDefaultAsync();

            response.SkillNames = await _dataContext.Skills
                                       .Where(x => response.SkillIds.Contains(x.Id))
                                       .Select(x => x.Name).ToListAsync();

            response.DocumentDetails = await _dataContext.Documents
                                       .Where(x => x.IdentificationId == response.Id
                                            && x.DocumentType == Constants.DocumentType.Cv)
                                       .Select(x => new FileDetailModel
                                       {
                                           Id = x.Id,
                                           IdentificationId = x.IdentificationId,
                                           Name = x.Name,
                                           Key = x.Key,
                                           DocumentType = x.DocumentType
                                       }).FirstOrDefaultAsync();
            return response;

        }


    }
}

