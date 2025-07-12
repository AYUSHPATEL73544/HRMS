using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models.Employee;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class EmployeeManagerRepository : IEmployeeManagerRepository
    {
        private readonly DataContext _dataContext;
        public EmployeeManagerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Team entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<List<TeamModel>> GetByEmployeeIdAsync(int employeeId)
        {
            return await (from e in _dataContext.Employees
                          join t in _dataContext.Teams on e.Id equals t.EmployeeId
                          where t.EmployeeId == employeeId
                          && t.Status != Constants.RecordStatus.Deleted
                          select new TeamModel
                          {
                              Id = t.Id,
                              EmployeeId = e.Id,
                              ManagerId = t.ManagerId,
                              Type = t.Type,
                              Status = t.Status
                          }).OrderByDescending(t => t.Id)
                          .ToListAsync();
        }

        public async Task<bool> IsManagerAssignedAsync(int employeeId, int managerId)
        {
            return await (from t in _dataContext.Teams
                          join e in _dataContext.Employees on t.EmployeeId equals e.Id
                          where t.ManagerId == managerId 
                          && t.EmployeeId == employeeId
                          && t.Status == Constants.RecordStatus.Active
                          select t.Id).AnyAsync();
        }

        public async Task<TeamModel> GetByIdAsync(int id)
        {
            return await _dataContext.Teams
                .Where(x => x.Id == id
                && x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new TeamModel
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    ManagerId = x.ManagerId,
                    Type = x.Type,
                    Status = x.Status
                }).SingleAsync();
        }

        public async Task<List<TeamReportessModel>>GetByManagerIdAsync(int id)
        {
            return await (from t in _dataContext.Teams
                          join e in _dataContext.Employees on t.EmployeeId equals e.Id
                          where (t.ManagerId == id && t.Status != Constants.RecordStatus.Deleted && e.Status != Constants.RecordStatus.Inactive)
                          select new TeamReportessModel
                          {
                              teamId = t.Id,
                              Id = e.Id,
                              ManagerId = t.ManagerId,
                              EmployeeName = e.FirstName + " "+ e.LastName,
                              Type = t.Type,
                              Status = t.Status
                          }).Distinct()
                          .OrderByDescending(t => t.teamId)
                          .ToListAsync();
        }

        public async Task<List<int>> GetEmployeeIdByManagerId(int id)
        {
            return await _dataContext.Teams.Where(x => x.ManagerId == id
                          && x.Status ==Constants.RecordStatus.Active ).Select(x => x.EmployeeId).ToListAsync();
        }

        public async Task<Team> FindAsync(int id)
        {
            return await _dataContext.Teams.FindAsync(id);
        }

        public void Update(Team entity)
        {
            _dataContext.Teams.Update(entity);
        }
    }
}
