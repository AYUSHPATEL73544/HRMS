using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Employee;
using Hrms.Core.Models.JobApplication;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System;
using System.Data;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _dataContext;
        public EmployeeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> IsCodeExistAsync(string code)
        {
            return await _dataContext.Employees
                .AsNoTracking()
                .AnyAsync(x => x.Code.Equals(code)
                    && (x.Status != Constants.RecordStatus.Deleted
                    || x.Status != Constants.RecordStatus.Inactive));
        }

        public async Task AddAsync(Employee entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<EmployeeModel> GetAsync(int id)
        {
            return await (from e in _dataContext.Employees
                          join d in _dataContext.Departments on e.DepartmentId equals d.Id into dep
                          from d in dep.DefaultIfEmpty()
                          join ds in _dataContext.Designations on e.DesignationId equals ds.Id into des
                          from ds in des.DefaultIfEmpty()
                          where e.Id == id
                       && e.Status != Constants.RecordStatus.Deleted
                          select new EmployeeModel
                          {
                              Id = e.Id,
                              Code = e.Code,
                              UserId = e.UserId,
                              CompanyId = e.CompanyId,
                              DesignationId = e.DesignationId,
                              DesignationName = ds.Name,
                              DepartmentId = e.DepartmentId,
                              DepartmentName = d.Name,
                              ManagerId = e.ManagerId,
                              BloodGroup = e.BloodGroup,
                              MaritalStatus = e.MaritalStatus,
                              FirstName = e.FirstName,
                              LastName = e.LastName,
                              Gender = e.Gender,
                              Email = e.Email,
                              Phone = e.Phone,
                              AlternateEmail = e.AlternateEmail,
                              AlternatePhone = e.AlternatePhone,
                              DateOfBirth = e.DateOfBirth,
                              DateOfJoining = e.DateOfJoining,
                              DateOfLeaving = e.DateOfLeaving,
                              Status = e.Status,
                              NoticePeriod = e.NoticePeriod,
                              Note = e.Note,
                              ExitDate = e.ExitDate,
                              EmployeeType = e.EmployeeType,
                              ProbationPeriod = e.ProbationPeriod
                          }).SingleOrDefaultAsync();
        }

        public async Task<EmployeeModel> GetByUserIdAsync(int userId)
        {
            var res =  await (from x in _dataContext.Employees
                          join d in _dataContext.Departments on x.DepartmentId equals d.Id into dep
                          from d in dep.DefaultIfEmpty()
                          join ds in _dataContext.Designations on x.DesignationId equals ds.Id into des
                          from ds in des.DefaultIfEmpty()
                          where x.UserId == userId
                           && x.Status != Constants.RecordStatus.Deleted
                          select new EmployeeModel
                          {
                              Id = x.Id,
                              Code = x.Code,
                              UserId = x.UserId,
                              CompanyId = x.CompanyId,
                              DesignationId = x.DesignationId,
                              DesignationName = ds.Name,
                              DepartmentId = x.DepartmentId,
                              DepartmentName = d.Name,
                              ManagerId = x.ManagerId,
                              BloodGroup = x.BloodGroup,
                              MaritalStatus = x.MaritalStatus,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                              Gender = x.Gender,
                              Email = x.Email,
                              Phone = x.Phone,
                              AlternateEmail = x.AlternateEmail,
                              AlternatePhone = x.AlternatePhone,
                              DateOfBirth = x.DateOfBirth,
                              DateOfJoining = x.DateOfJoining,
                              DateOfLeaving = x.DateOfLeaving,
                              Status = x.Status,
                              ProbationPeriod = x.ProbationPeriod,
                              EmployeeType = x.EmployeeType,
                              ExitDate = x.ExitDate,
                              Note = x.Note,
                              NoticePeriod = x.NoticePeriod
                          }).SingleOrDefaultAsync();

            if (res != null)
            {
                res.ImageDetails = await (from d in _dataContext.Documents
                                          where (d.IdentificationId == res.UserId
                                          && d.DocumentType == Constants.DocumentType.ProfileImage)
                                          select new FileDetailModel
                                          {
                                              Id = d.Id,
                                              IdentificationId = userId,
                                              Name = d.Name,
                                              Key = d.Key,
                                          }).SingleOrDefaultAsync();

            }

            return res;
        }



        public async Task<List<EmployeeModel>> GetByDepartmentId(int departmentId)
        {
            return await (from e in _dataContext.Employees
                          where e.DepartmentId == departmentId
                          select new EmployeeModel
                          {
                              EmployeeName = e.FirstName + " " + e.LastName,
                              DepartmentId = e.DepartmentId
                          }).ToListAsync();
        }

        public async Task<List<EmployeeModel>> GetByDesignationId(int designationId)
        {
            return await (from e in _dataContext.Employees
                          where e.DesignationId == designationId
                          select new EmployeeModel
                          {
                              EmployeeName = e.FirstName + " " + e.LastName,
                              DesignationId = e.DesignationId
                          }).ToListAsync();
        }
        public async Task<int> GetUserIdAsync(int id)
        {
            return await (_dataContext.Employees
                        .Where(x => x.Id == id
                          && x.Status != Constants.RecordStatus.Deleted)
                        .Select(x => x.UserId)).FirstAsync();
        }

        public async Task<int> GetIdByUserIdAsync(int userId)
        {
            return await _dataContext.Employees
                   .Where(x => x.UserId == userId
                   && x.Status != Constants.RecordStatus.Deleted)
                   .Select(x => x.Id).SingleAsync();
        }

        public async Task<string> GetNameByIdAsync(int id)
        {
            return  await (from employee in _dataContext.Employees
                          where employee.Id == id
                          select employee.FirstName + " " + employee.LastName).SingleAsync();
             
        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _dataContext.Employees
                .Where(x => x.Status != Constants.RecordStatus.Inactive)
                .OrderBy(x => x.FirstName)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.FirstName + " " + x.LastName
                }).ToListAsync();
        }

        public async Task<List<SelectListItemModel>> GetManagerListAsync(int userId)
        {
            return await (from u in _dataContext.Users
                          join e in _dataContext.Employees on u.Id equals e.UserId
                          join r in _dataContext.UserRoles on u.Id equals r.UserId
                          where u.Id != userId && r.RoleId == 5
                          select new SelectListItemModel
                          {
                              Key = e.Id,
                              Value = u.FirstName + " " + u.LastName
                          }).OrderBy(x => x.Value)
                        .ToListAsync();

        }

        public async Task<IEnumerable<SelectListItemModel>> GetEmployeeListItemByLeaveRuleIdAsync(int leaveRuleId)
        {
            var employees = await (from e in _dataContext.Employees
                                   where !_dataContext.Leaves.Any(x => x.EmployeeId == e.Id
                                            && x.RuleId == leaveRuleId
                                            && x.Status == Constants.RecordStatus.Active)
                                   && e.Status != Constants.RecordStatus.Inactive
                                   select new SelectListItemModel
                                   {
                                       Key = e.Id,
                                       Value = e.FirstName + " " + e.LastName

                                   }).Distinct()
                                    .OrderBy(x => x.Value)
                                    .ToListAsync();

            return employees;
        }

        public async Task<IEnumerable<SelectListItemModel>> GetEmployeeListByAttendanceRuleIdAsync(int attendanceRuleId)
        {
            var employees = await (from e in _dataContext.Employees
                                   where !_dataContext.EmployeeAttendanceRules.Any(x => x.EmployeeId == e.Id
                                            && x.AttendanceRuleId == attendanceRuleId
                                            && x.Status == Constants.RecordStatus.Active)
                                   && e.Status != Constants.RecordStatus.Inactive
                                   select new SelectListItemModel
                                   {
                                       Key = e.Id,
                                       Value = e.FirstName + " " + e.LastName

                                   }).OrderBy(x => x.Value)
                                   .ToListAsync();


            return employees;

        }

        public async Task<List<EmployeeModel>> GetListAsync()
        {
            return await _dataContext.Employees
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new EmployeeModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    UserId = x.UserId,
                    CompanyId = x.CompanyId,
                    DesignationId = x.DesignationId,
                    DepartmentId = x.DepartmentId,
                    ManagerId = x.ManagerId,
                    Email = x.Email,
                    Phone = x.Phone,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BloodGroup = x.BloodGroup,
                    MaritalStatus = x.MaritalStatus,
                    Gender = x.Gender,
                    AlternateEmail = x.AlternateEmail,
                    AlternatePhone = x.AlternatePhone,
                    DateOfBirth = x.DateOfBirth,
                    DateOfJoining = x.DateOfJoining,
                    DateOfLeaving = x.DateOfLeaving,
                    Status = x.Status
                }).ToListAsync();
        }

        public async Task<List<EmployeeModel>> GetActiveEmployeeListAsync()
        {
            return await _dataContext.Employees
                .Where(x => x.Status != Constants.RecordStatus.Deleted && x.Status == Constants.RecordStatus.Active)
                .Select(x => new EmployeeModel
                {
                    Id = x.Id,
                    DateOfJoining = x.DateOfJoining,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Code = x.Code,
                    DepartmentId = x.DepartmentId,

                }).ToListAsync();
        }

        //public async Task<EmployeeModel> GetActiveEmployeeListAsync(int employeeId)
        //{
        //    return await _dataContext.Employees
        //        .Where(x => x.Status != Constants.RecordStatus.Deleted 
        //        && x.Status == Constants.RecordStatus.Active && x.Id == employeeId)

        //        .Select(x => new EmployeeModel
        //        {
        //            Id = x.Id,
        //            DepartmentId = x.DepartmentId,

        //        }).SingleOrDefaultAsync();
        //}

        public async Task<MatTableResponse<EmployeeModel>> GetPageListAsync(MatDataTableRequest model, int employeeStatus)
        {
            var sortExpression = model.SortExpression();
            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from e in _dataContext.Employees
                           join d in _dataContext.Departments on e.DepartmentId equals d.Id into departmentJoin
                           from d in departmentJoin.DefaultIfEmpty()
                           join dn in _dataContext.Designations on e.DesignationId equals dn.Id into designationJoin
                           from dn in designationJoin.DefaultIfEmpty()
                           where ((int)e.Status == employeeStatus || e.Status == Constants.RecordStatus.Active) && e.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName + " " + e.LastName, "%" + model.FilterKey + "%")
                            || EF.Functions.Like(e.FirstName + e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Email, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.AlternateEmail, "%" + model.FilterKey + "%"))
                           select new EmployeeModel
                           {
                               Id = e.Id,
                               UserId = e.UserId,
                               Code = e.Code,
                               FirstName = e.FirstName,
                               LastName = e.LastName,
                               Email = e.Email,
                               DepartmentId = e.DepartmentId,
                               DesignationId = e.DesignationId,
                               AlternateEmail = e.AlternateEmail,
                               Phone = e.Phone,
                               AlternatePhone = e.AlternatePhone,
                               ManagerId = e.ManagerId,
                               Department = d != null ? d.Name : null,
                               Designation = dn != null ? dn.Name : null,
                               Status = e.Status,
                               CreatedOn = e.CreatedOn
                           };

            var response = new MatTableResponse<EmployeeModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };


            foreach (var employee in response.Items)
            {
                if (employee.DepartmentId != 0)
                {
                    employee.Department = await (from d in _dataContext.Departments
                                                 where d.Id == employee.DepartmentId
                                                 select d.Name).SingleOrDefaultAsync();
                }
                if (employee.DesignationId != 0)
                {
                    employee.Designation = await (from d in _dataContext.Designations
                                                  where d.Id == employee.DesignationId
                                                  select d.Name).SingleOrDefaultAsync();
                }
                if (employee.ManagerId != null && employee.ManagerId != 0)
                {
                    employee.ManagerName = await (from e in _dataContext.Employees
                                                  where e.Id == employee.ManagerId
                                                  && e.Status != Constants.RecordStatus.Deleted
                                                  select $"{e.FirstName} {e.LastName}").SingleOrDefaultAsync();
                }

                employee.ImageDetails = await (from doc in _dataContext.Documents
                                               where doc.IdentificationId == employee.UserId
                                               && doc.DocumentType == Constants.DocumentType.ProfileImage
                                               select new FileDetailModel
                                               {
                                                   Id = doc.Id,
                                                   IdentificationId = doc.IdentificationId,
                                                   Name = doc.Name,
                                                   Key = doc.Key
                                               })
                                              .SingleOrDefaultAsync();

            }
            return response;
        }

        public async Task<MatTableResponse<EmployeeModel>> GetInactiveEmployeeListAsync(MatDataTableRequest model)
        {
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from e in _dataContext.Employees
                           join d in _dataContext.Departments on e.DepartmentId equals d.Id into departmentJoin
                           from d in departmentJoin.DefaultIfEmpty()
                           join dn in _dataContext.Designations on e.DesignationId equals dn.Id into designationJoin
                           from dn in designationJoin.DefaultIfEmpty()
                           where e.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(e.FirstName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Code, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.LastName, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.Email, "%" + model.FilterKey + "%")
                           || EF.Functions.Like(e.AlternateEmail, "%" + model.FilterKey + "%"))
                           select new EmployeeModel
                           {
                               Id = e.Id,
                               Code = e.Code,
                               FirstName = e.FirstName,
                               LastName = e.LastName,
                               Email = e.Email,
                               DepartmentId = e.DepartmentId,
                               DesignationId = e.DesignationId,
                               AlternateEmail = e.AlternateEmail,
                               Phone = e.Phone,
                               AlternatePhone = e.AlternatePhone,
                               ManagerId = e.ManagerId,
                               Department = d != null ? d.Name : null,
                               Designation = dn != null ? dn.Name : null,
                               Status = e.Status,
                               CreatedOn = e.CreatedOn
                           };

            var response = new MatTableResponse<EmployeeModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };

            foreach (var employee in response.Items)
            {
                if (employee.DepartmentId != 0)
                {
                    employee.Department = await (from d in _dataContext.Departments
                                                 where d.Id == employee.DepartmentId
                                                 select d.Name).SingleOrDefaultAsync();
                }
                if (employee.DesignationId != 0)
                {
                    employee.Designation = await (from d in _dataContext.Designations
                                                  where d.Id == employee.DesignationId
                                                  select d.Name).SingleOrDefaultAsync();
                }
                if (employee.ManagerId != null && employee.ManagerId != 0)
                {
                    employee.ManagerName = await (from e in _dataContext.Employees
                                                  where e.Id == employee.ManagerId
                                                  && e.Status != Constants.RecordStatus.Deleted
                                                  select $"{e.FirstName} {e.LastName}").SingleOrDefaultAsync();
                }
            }
            return response;
        }

        public async Task<List<CompanyEventsModel>> GetCalendarEventAsync(int year, int month)
        {
            var linq = await (from e in _dataContext.Employees
                              join l in _dataContext.LeaveLogs on e.Id equals l.EmployeeId into leaves
                              from leave in leaves.DefaultIfEmpty()
                              where e.Status == Constants.RecordStatus.Active
                                    && ((e.DateOfBirth != null && e.DateOfBirth.Value.Month == month)
                                    || (leave == null
                                    || (leave != null && leave.Status == Constants.RecordStatus.Approved
                                    || leave != null && leave.Status == Constants.RecordStatus.Pending
                                    && leave.StartDate.Year == year && leave.StartDate.Month == month)))
                              select new CompanyEventsModel
                              {
                                  Title = e.FirstName + " " + e.LastName,
                                  DateOfBirth = e.DateOfBirth,
                                  Status = leave != null ? leave.Status : null,
                                  Start = leave != null ? leave.StartDate : null,
                                  End = leave != null ? leave.EndDate : null,
                                  IsHalfDay = (leave.StartHalf != 0 && leave.EndHalf != 0) && leave.StartHalf == leave.EndHalf,
                              }).ToListAsync();



            return linq;
        }

        public async Task<List<CompanyEventsModel>> GetLeaveLogCalendarEventAsync(int year, int month, int employeeId)
        {
            var res = await ( from l in _dataContext.LeaveLogs 
                             where (l.EmployeeId == employeeId
                             && l.StartDate.Year == year && l.EndDate.Month == month
                             )
                             select new CompanyEventsModel
                             {
                                 Status = l != null ? l.Status : null,
                                 Start = l != null ? l.StartDate : null,
                                 End = l != null ? l.EndDate : null,
                                 IsHalfDay = (l.StartHalf != 0 && l.EndHalf != 0) && l.StartHalf == l.EndHalf,
                             }).ToListAsync();
            return res;
        }

        public async Task<Employee> FindAsync(int id)
        {
            return await _dataContext.Employees.FindAsync(id);
        }

        public async Task<int> LastEmployeeAsync()
        {
            var maxId = await _dataContext.Employees
        .OrderByDescending(x => x.Id) 
        .Select(x => x.Id)
        .FirstOrDefaultAsync(); 

            return maxId;
        }

        public void Update(Employee entity)
        {
            _dataContext.Employees.Update(entity);
        }

    }
}
