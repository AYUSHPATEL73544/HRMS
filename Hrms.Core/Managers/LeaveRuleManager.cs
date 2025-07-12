using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using System.Data;

namespace Hrms.Core.Managers
{
    public class LeaveRuleManager : ILeaveRuleManager
    {
        private readonly ILeaveRuleRepository _leaveRuleRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveLogRepository _leaveLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LeaveRuleManager(ILeaveRuleRepository leaveRuleRepository,
            ILeaveRepository leaveRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork,
            ILeaveLogRepository leaveLogRepository)
        {
            _leaveRuleRepository = leaveRuleRepository;
            _leaveRepository = leaveRepository;
            _employeeRepository = employeeRepository;
            _leaveLogRepository = leaveLogRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task AddAsync(LeaveRuleModel model)
        {
                var leaveRule = new LeaveRule
                {
                    CompanyId = model.CompanyId,
                    Title = model.Title,
                    Description = model.Description,
                    MaxAllowedInYear = model.MaxAllowedInYear,
                    MaxAllowedInMonth = model.MaxAllowedInMonth,
                    MaxBackDatedLeavesAllowed = model.MaxBackDatedLeavesAllowed,
                    MaxAllowedContinues = model.MaxAllowedContinues,
                    CountWeekendAsLeave = model.CountWeekendAsLeave,
                    CountHolidayAsLeave = model.CountHolidayAsLeave,
                    AccrualFrequency = model.AccrualFrequency,
                    AccrualPeriod = model.AccrualPeriod,
                    CreditableOnAccrualBasis = model.CreditableOnAccrualBasis,
                    AllowedUnderProbation = model.AllowedUnderProbation,
                    AllowedNegative = model.AllowedNegative,
                    AllowedCarryForward = model.AllowedCarryForward,
                    AllowedDonation = model.AllowedDonation,
                    AllowedBackDatedLeaves = model.AllowedBackDatedLeaves,
                    ApplyTillNextYear = model.ApplyTillNextYear,
                    FutureDatedLeavesAllowed = model.FutureDatedLeavesAllowed,
                    FutureDatedLeavesAllowedUpTo = model.FutureDatedLeavesAllowedUpTo,
                    Status = Constants.RecordStatus.Active,
                    EffectiveFrom = Utility.GetDateTime(),
                    CreatedOn = Utility.GetDateTime()
                };
                await _leaveRuleRepository.AddAsync(leaveRule);
                await _unitOfWork.SaveChangesAsync();
           
        }

        public async Task<LeaveRuleModel> GetAsync(int id)
        {
            return await _leaveRuleRepository.GetAsync(id);
        }

        public async Task<MatTableResponse<LeaveRuleModel>> GetListAsync(MatDataTableRequest model, int userId)
        {
            var employee = await _employeeRepository.GetByUserIdAsync(userId);
            return await _leaveRuleRepository.GetListAsync(model, employee.Id);
        }

        public async Task<MatTableResponse<LeaveRuleModel>> GetPageListAsync(MatDataTableRequest model)
        {
            return await _leaveRuleRepository.GetPageListAsync(model);
        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _leaveRuleRepository.GetSelectListItemAsync();
        }

        public async Task UpdateAsync(LeaveRuleModel model)
        {
            var leaveRule = await _leaveRuleRepository.FindAsync(model.Id);

            leaveRule.CompanyId = model.CompanyId;
            leaveRule.Title = model.Title;
            leaveRule.Description = model.Description;
            leaveRule.MaxAllowedInYear = model.MaxAllowedInYear;
            leaveRule.MaxAllowedInMonth = model.MaxAllowedInMonth;
            leaveRule.MaxAllowedContinues = model.MaxAllowedContinues;
            leaveRule.MaxBackDatedLeavesAllowed = model.MaxBackDatedLeavesAllowed;
            leaveRule.CountWeekendAsLeave = model.CountWeekendAsLeave;
            leaveRule.CountHolidayAsLeave = model.CountHolidayAsLeave;
            leaveRule.CreditableOnAccrualBasis = model.CreditableOnAccrualBasis;
            leaveRule.AccrualFrequency = model.AccrualFrequency;
            leaveRule.AccrualPeriod = model.AccrualPeriod;
            leaveRule.AllowedUnderProbation = model.AllowedUnderProbation;
            leaveRule.AllowedNegative = model.AllowedNegative;
            leaveRule.AllowedCarryForward = model.AllowedCarryForward;
            leaveRule.AllowedDonation = model.AllowedDonation;
            leaveRule.ApplyTillNextYear = model.ApplyTillNextYear;
            leaveRule.FutureDatedLeavesAllowedUpTo = model.FutureDatedLeavesAllowedUpTo;
            leaveRule.FutureDatedLeavesAllowed = model.FutureDatedLeavesAllowed;
            leaveRule.AllowedBackDatedLeaves = model.AllowedBackDatedLeaves;

            _leaveRuleRepository.Update(leaveRule);
            await _unitOfWork.SaveChangesAsync();

            var leaves = await _leaveRepository.GetByRuleIdAsync(leaveRule.Id);

            foreach(var leave in leaves)
            {
                var employee = await _employeeRepository.GetAsync(leave.EmployeeId);

                var leavesToCredit = _leaveRepository.CalculateLeavesToCredit(employee.DateOfJoining, leaveRule.MaxAllowedInYear);


                leave.Total = leaveRule.MaxAllowedInYear; 
                leave.Credited = leavesToCredit;
                leave.Available = leave.Credited - leave.Applied;

                _leaveRepository.Update(leave);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _leaveRuleRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            entity.EffectiveTo = Utility.GetDateTime();
            _leaveRuleRepository.Update(entity);

            var leaveLog = await _leaveLogRepository.GetByRuleIdAsync(id);
            if (leaveLog != null)
            {
                foreach (var log in leaveLog)
                {
                    log.EffectiveTo = Utility.GetDateTime();
                    _leaveLogRepository.Update(log);
                }
            }

            var leaves = await _leaveRepository.GetByRuleIdAsync(entity.Id);

            if (leaves != null)
            {
                foreach (var leave in leaves)
                {
                    leave.EffectiveTo = Utility.GetDateTime();
                    _leaveRepository.Update(leave);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
