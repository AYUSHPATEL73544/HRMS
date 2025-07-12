using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Entities;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Hrms.Core.Models;

namespace Hrms.Core.Managers
{
    public class LeaveManager : ILeaveManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveRuleRepository _leaveRuleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILeaveLogRepository _leaveLogRepository;

        public LeaveManager(ILeaveRepository leaveRepository,
            ILeaveRuleRepository leaveRuleRepository,
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            ILeaveLogRepository leaveLogRepository,
            IUnitOfWork unitOfWork)
        {
            _leaveRepository = leaveRepository;
            _leaveRuleRepository = leaveRuleRepository;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _leaveLogRepository = leaveLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(LeaveModel model)
        {
            foreach (var employeeId in model.EmployeeIds)
            {
                decimal prevAppliedLeaves = 0;
                var rule = await _leaveRuleRepository.GetAsync(model.RuleId);
                var employee = await _employeeRepository.GetAsync(employeeId);
                var leavesToCredit = _leaveRepository.CalculateLeavesToCredit(employee.DateOfJoining, rule.MaxAllowedInYear);
                var leave = new Leave
                {
                    EmployeeId = employeeId,
                    RuleId = model.RuleId,
                    Total = rule.MaxAllowedInYear,
                    Credited = leavesToCredit,
                    Available = leavesToCredit - prevAppliedLeaves,
                    Applied = prevAppliedLeaves,
                    Status = Constants.RecordStatus.Active,
                    EffectiveFrom = Utility.GetDateTime(),
                    CreatedOn = Utility.GetDateTime()
                };
                await _leaveRepository.AddAsync(leave);

            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<LeaveModel> GetAsync(int id)
        {
            return await _leaveRepository.GetAsync(id);
        }

        public async Task<List<LeaveModel>> GetDetailAsync()
        {
            return await _leaveRepository.GetListAsync();
        }

        public async Task<List<LeaveModel>> GetLeaveBalanceListAsync()
        {
            var leaves = await _leaveRepository.GetListAsync();
            if (leaves != null)
            {
                foreach (var leave in leaves)
                {
                    var employee = await _employeeRepository.GetAsync(leave.EmployeeId);
                    var rule = await _leaveRuleRepository.GetAsync(leave.RuleId);
                    var department = await _departmentRepository.GetDetailAsync(employee.DepartmentId);
                    leave.EmployeeName = employee.FirstName + " " + employee.LastName;
                    leave.EmployeeCode = employee.Code;
                    leave.LeaveRule = rule.Title;
                    leave.Department = department.Name;
                }
            }
            return leaves;
        }

        public async Task<List<LeaveModel>> GetListAsync(int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);
            return await _leaveRepository.GetListAsync(employee);
        }

        public async Task<MatTableResponse<LeaveModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            return await _leaveRepository.GetPagedListAsync(model);
        }

        public async Task<MatTableResponse<LeaveModel>> GetAssignRuleListAsync(MatDataTableRequest model)
        {
            var leaves = await _leaveRepository.GetAssignRuleListAsync(model);

            if (leaves != null)
            {
                foreach (var leave in leaves.Items)
                {
                    var rules = await _leaveRuleRepository.GetListByEmployeeIdAsync(leave.EmployeeId);
                    var department = await _departmentRepository.GetDetailAsync(leave.DepartmentId);
                    if (department != null)
                    {
                        leave.Department = department.Name;
                    }
                    if (rules != null)
                    {
                        leave.LeaveRules = rules;
                    }

                }
            }

            return leaves;
        }

        public async Task<MatTableResponse<LeaveModel>> GetInactiveAssignListAsync(MatDataTableRequest model)
        {
            var leaves = await _leaveRepository.GetInactiveAssignListAsync(model);

            if (leaves != null)
            {
                foreach (var leave in leaves.Items)
                {
                    var rules = await _leaveRuleRepository.GetListByEmployeeIdAsync(leave.EmployeeId);
                    var department = await _departmentRepository.GetDetailAsync(leave.DepartmentId);
                    if (department != null)
                    {
                        leave.Department = department.Name;
                    }
                    if (rules != null)
                    {
                        leave.LeaveRules = rules;
                    }

                }
            }

            return leaves;
        }

        public async Task<Leave> GetByRuleId(int ruleId, int userId)
        {
            var employee = await _employeeRepository.GetByUserIdAsync(userId);
            return await _leaveRepository.GetByRuleIdAsync(employee.Id, ruleId);
        }

        public async Task UpdateAsync(LeaveModel model)
        {
            var leave = await _leaveRepository.FindAsync(model.Id);

            leave.EmployeeId = model.EmployeeId;
            leave.RuleId = model.RuleId;
            leave.Total = model.Total;
            leave.Credited = model.Credited;
            leave.Available = model.Available;
            leave.Applied = model.Applied;

            _leaveRepository.Update(leave);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int employeeId, int ruleId)
        {
            var entity = await _leaveRepository.GetByRuleIdAsync(employeeId, ruleId);

            entity.Status = Constants.RecordStatus.Deleted;
            entity.EffectiveTo = Utility.GetDateTime();
            
            var leavelogs = await _leaveLogRepository.GetByRuleIdEmployeeId(ruleId , employeeId);
            foreach (var log in leavelogs)
            {
                log.Status = Constants.RecordStatus.Deleted;
            }

             _leaveLogRepository.Update(leavelogs);

            _leaveRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
