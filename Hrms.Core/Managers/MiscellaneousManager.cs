using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Utilities;


namespace Hrms.Core.Managers
{
    public class MiscellaneousManager : IMiscellaneousManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveRuleRepository _leaveRuleRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public MiscellaneousManager(ILeaveRepository leaveRepository,
            ILeaveRuleRepository leaveRuleRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _leaveRepository = leaveRepository;
            _leaveRuleRepository = leaveRuleRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateLeavesAsync()
        {
            var employees = await _employeeRepository.GetActiveEmployeeListAsync();
            var ruleIds = await _leaveRuleRepository.GetRuleIdListAsync();

            foreach (var employee in employees)
            {
                foreach (var ruleId in ruleIds)
                {
                    var leave = await _leaveRepository.GetByRuleIdAsync(employee.Id, ruleId);
                    if(leave == null)
                    {
                        continue;
                    }
                    var rule = await _leaveRuleRepository.GetAsync(ruleId);
                   
                   
                    if(rule.MaxAllowedInYear == 365 || rule.MaxAllowedInYear == 366)
                    {
                        var leavesToCredit = _leaveRepository.CalculateLossOfPayCreditLeaves();
                        leave.Credited += leavesToCredit;
                        leave.Available = leavesToCredit - leave.Applied;
                    }
                    else
                    {
                        var leavesToCredit = _leaveRepository.CalculateLeavesToCredit(employee.DateOfJoining, rule.MaxAllowedInYear);
                        leave.Credited = leavesToCredit;
                        leave.Available = leavesToCredit - leave.Applied;
                    }

                    leave.Total = rule.MaxAllowedInYear;
                    _leaveRepository.Update(leave);
                }
            }
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
