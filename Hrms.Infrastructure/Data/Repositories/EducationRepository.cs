using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class EducationRepository : IEducationRepository
    {
        private readonly DataContext _dataContext;
        public EducationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Education entity)
        {
            await _dataContext.AddAsync(entity);
        }


        public async Task<EducationModel> GetByIdAsync(int id)
        {
            return await _dataContext.Educations
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new EducationModel
                {
                    Id = x.Id,
                    CourseTypeId = x.CourseTypeId,
                    QualificationTypeId = x.QualificationTypeId,
                    CollegeName = x.CollegeName,
                    UniversityName = x.UniversityName,
                    CourseName = x.CourseName,
                    Start = x.Start,
                    EmployeeId = x.EmployeeId,
                    End = x.End,
                    Stream = x.Stream,
                    Status = x.Status,
                    CreatedOn = x.CreatedOn
                }).SingleAsync();
        }

        public async Task<MatTableResponse<EducationModel>> GetPageListAsync(MatDataTableRequest model, int employeeId)
        {
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from e in _dataContext.Educations
                           where e.EmployeeId == employeeId &&
                           e.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.UniversityName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.CollegeName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.CourseName, "%" + model.FilterKey + "%"))
                           select new EducationModel
                           {
                               Id = e.Id,
                               CourseTypeId = e.CourseTypeId,
                               QualificationTypeId = e.QualificationTypeId,
                               CollegeName = e.CollegeName,
                               UniversityName = e.UniversityName,
                               Start = e.Start,
                               CourseName = e.CourseName,
                               EmployeeId = e.EmployeeId,
                               End = e.End,
                               Stream = e.Stream,
                               Status = e.Status,
                               CreatedOn = e.CreatedOn,

                           };

            var response = new MatTableResponse<EducationModel>
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

        public async Task<Education> FindAsync(int id)
        {
            return await _dataContext.Educations.FindAsync(id);
        }

        public void Update(Education entity)
        {
            _dataContext.Educations.Update(entity);
        }
    }
}
