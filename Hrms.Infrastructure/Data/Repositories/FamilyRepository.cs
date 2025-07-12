using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly DataContext _dataContext;
        public FamilyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Family entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<List<FamilyModel>> GetAsync()
        {
            return await _dataContext.Families
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new FamilyModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    RelationshipId = x.RelationshipId,
                    Email = x.Email,
                    Phone = x.Phone,
                    DateOfBirth = x.DateOfBirth
                }).ToListAsync();
        }

        public async Task<FamilyModel> GetByIdAsync(int id)
        {
            return await _dataContext.Families
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new FamilyModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    RelationshipId = x.RelationshipId,
                    Email = x.Email,
                    Phone = x.Phone,
                    DateOfBirth = x.DateOfBirth
                }).SingleAsync();
        }

        public async Task<MatTableResponse<FamilyModel>> GetPagedListAsync(MatDataTableRequest model, int employeeId)
        {
                var sortExpression = model.SortExpression();

                var recordsToSkip = model.RecordsToSkip();

                var linqStmt = from f in _dataContext.Families
                               where f.EmployeeId == employeeId &&f.Status != Constants.RecordStatus.Deleted
                               && (model.FilterKey == null
                               || EF.Functions.Like(f.FirstName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(f.LastName, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(f.Email, "%" + model.FilterKey + "%")
                               || EF.Functions.Like(f.Phone, "%" + model.FilterKey + "%"))
                               select new FamilyModel
                               {
                                   Id = f.Id,
                                   FirstName = f.FirstName,
                                   LastName = f.LastName,
                                   RelationshipId = f.RelationshipId,
                                   DateOfBirth = f.DateOfBirth,
                                   Email = f.Email,
                                   Phone = f.Phone,
                               };

                var response = new MatTableResponse<FamilyModel>
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

        public async Task<Family> FindAsync(int id)
        {
            return await _dataContext.Families.FindAsync(id);
        }

        public void Update(Family entity)
        {
            _dataContext.Families.Update(entity);
        }
    }
}
