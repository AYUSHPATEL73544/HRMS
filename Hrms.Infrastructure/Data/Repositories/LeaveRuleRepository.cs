using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hrms.Infrastructure.Data.Repositories
{
    public class LeaveRuleRepository : ILeaveRuleRepository
    {
        private readonly DataContext _dataContext;
        public LeaveRuleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(LeaveRule entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public async Task<LeaveRuleModel> GetAsync(int id)
        {
            return await _dataContext.LeaveRules
                .Where(x => x.Id == id 
                && x.Status == Constants.RecordStatus.Active)
                .Select(x => new LeaveRuleModel
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Title = x.Title,
                    Description = x.Description,
                    MaxAllowedInYear = x.MaxAllowedInYear,
                    MaxAllowedInMonth = x.MaxAllowedInMonth,
                    MaxAllowedContinues = x.MaxAllowedContinues,
                    MaxBackDatedLeavesAllowed = x.MaxBackDatedLeavesAllowed,
                    CountWeekendAsLeave = x.CountWeekendAsLeave,
                    CountHolidayAsLeave = x.CountHolidayAsLeave,
                    CreditableOnAccrualBasis = x.CreditableOnAccrualBasis,
                    AccrualFrequency = x.AccrualFrequency,
                    AccrualPeriod = x.AccrualPeriod,
                    AllowedUnderProbation = x.AllowedUnderProbation,
                    AllowedNegative = x.AllowedNegative,
                    AllowedCarryForward = x.AllowedCarryForward,
                    AllowedDonation = x.AllowedDonation,
                    AllowedBackDatedLeaves = x.AllowedBackDatedLeaves,
                    ApplyTillNextYear = x.ApplyTillNextYear,
                    FutureDatedLeavesAllowed = x.FutureDatedLeavesAllowed,
                    FutureDatedLeavesAllowedUpTo = x.FutureDatedLeavesAllowedUpTo,
                    Status = x.Status
                }).SingleOrDefaultAsync();
        }

        public async Task<MatTableResponse<LeaveRuleModel>> GetListAsync(MatDataTableRequest model, int employeeId)
        {
            var currentDate = Utility.GetDateTime();
            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from lr in _dataContext.LeaveRules
                           join l in _dataContext.Leaves on lr.Id equals l.RuleId
                           where l.EmployeeId == employeeId &&
                            lr.EffectiveFrom <= currentDate 
                            && (lr.EffectiveTo == null || lr.EffectiveTo >= currentDate)
                           && lr.Status != Constants.RecordStatus.Deleted
                           && l.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(lr.Title, "%" + model.FilterKey + "%"))
                           select new LeaveRuleModel
                           {
                               Id = lr.Id,
                               CompanyId = lr.CompanyId,
                               Title = lr.Title,
                               Description = lr.Description,
                               MaxAllowedInYear = lr.MaxAllowedInYear,
                               MaxAllowedInMonth = lr.MaxAllowedInMonth,
                               MaxAllowedContinues = lr.MaxAllowedContinues,
                               MaxBackDatedLeavesAllowed = lr.MaxBackDatedLeavesAllowed,
                               CountWeekendAsLeave = lr.CountWeekendAsLeave,
                               CountHolidayAsLeave = lr.CountHolidayAsLeave,
                               CreditableOnAccrualBasis = lr.CreditableOnAccrualBasis,
                               AccrualFrequency = lr.AccrualFrequency,
                               AccrualPeriod = lr.AccrualPeriod,
                               AllowedUnderProbation = lr.AllowedUnderProbation,
                               AllowedNegative = lr.AllowedNegative,
                               AllowedCarryForward = lr.AllowedCarryForward,
                               AllowedDonation = lr.AllowedDonation,
                               FutureDatedLeavesAllowed = lr.FutureDatedLeavesAllowed,
                               FutureDatedLeavesAllowedUpTo = lr.FutureDatedLeavesAllowedUpTo,
                               AllowedBackDatedLeaves = lr.AllowedBackDatedLeaves,
                               ApplyTillNextYear = lr.ApplyTillNextYear,
                               Status = lr.Status,
                               CreatedOn = lr.CreatedOn
                           };

            var response = new MatTableResponse<LeaveRuleModel>
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

        public async Task<List<LeaveRuleModel>> GetListByEmployeeIdAsync(int employeeId)
        {
            return await (from lr in _dataContext.LeaveRules
                          join l in _dataContext.Leaves on lr.Id equals l.RuleId
                          where l.EmployeeId == employeeId 
                          && l.Status == Constants.RecordStatus.Active
                           select new LeaveRuleModel
                           {
                               Title = lr.Title,
                               Status = lr.Status,
                               LeaveRuleStatus = lr.Status,
                           }).ToListAsync();
        }


        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _dataContext.LeaveRules
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .Select(x => new SelectListItemModel
                {
                    Key = x.Id,
                    Value = x.Title
                }).ToListAsync();
        }

        public async Task<List<int>> GetRuleIdListAsync()
        {
            return await _dataContext.LeaveRules
                .Where(x => x.Status != Constants.RecordStatus.Deleted)
                .Select(x => x.Id).ToListAsync();
        }

        public async Task<MatTableResponse<LeaveRuleModel>> GetPageListAsync(MatDataTableRequest model)
        {
            var currentDate = Utility.GetDateTime();

            var sortExpression = model.SortExpression();

            var recordsToSkip = model.RecordsToSkip();

            var linqStmt = from lr in _dataContext.LeaveRules
                           where lr.EffectiveFrom <= currentDate 
                           && (lr.EffectiveTo == null || lr.EffectiveTo >= currentDate)
                           && lr.Status != Constants.RecordStatus.Deleted
                           && (model.FilterKey == null
                           || EF.Functions.Like(lr.Title, "%" + model.FilterKey + "%"))
                           select new LeaveRuleModel
                           {
                               Id = lr.Id,
                               CompanyId = lr.CompanyId,
                               Title = lr.Title,
                               Description = lr.Description,
                               MaxAllowedInYear = lr.MaxAllowedInYear,
                               MaxAllowedInMonth = lr.MaxAllowedInMonth,
                               MaxAllowedContinues = lr.MaxAllowedContinues,
                               MaxBackDatedLeavesAllowed = lr.MaxBackDatedLeavesAllowed,
                               CountWeekendAsLeave = lr.CountWeekendAsLeave,
                               CountHolidayAsLeave = lr.CountHolidayAsLeave,
                               CreditableOnAccrualBasis = lr.CreditableOnAccrualBasis,
                               AccrualFrequency = lr.AccrualFrequency,
                               AccrualPeriod = lr.AccrualPeriod,
                               AllowedUnderProbation = lr.AllowedUnderProbation,
                               AllowedNegative = lr.AllowedNegative,
                               AllowedCarryForward = lr.AllowedCarryForward,
                               AllowedDonation = lr.AllowedDonation,
                               AllowedBackDatedLeaves = lr.AllowedBackDatedLeaves,
                               ApplyTillNextYear = lr.ApplyTillNextYear,
                               FutureDatedLeavesAllowed = lr.FutureDatedLeavesAllowed,
                               FutureDatedLeavesAllowedUpTo = lr.FutureDatedLeavesAllowedUpTo,
                               Status = lr.Status, 
                               CreatedOn = lr.CreatedOn
                           };

            var response = new MatTableResponse<LeaveRuleModel>
            {
                TotalCount = await linqStmt.CountAsync(),
                Items = await linqStmt
                .OrderBy(sortExpression)
                    .Skip(recordsToSkip)
                    .Take(model.PageSize)
                    .ToListAsync()
            };

            foreach(var leave in response.Items)
            {
                leave.People = await (from l in _dataContext.Leaves
                                      join e in _dataContext.Employees on l.EmployeeId equals e.Id
                                      where l.RuleId == leave.Id
                                         && e.Status == Constants.RecordStatus.Active
                                         && l.Status == Constants.RecordStatus.Active
                                         select l.Id).CountAsync();
            }

            return response;
        }

        public async Task<LeaveRule> FindAsync(int id)
        {
            return await _dataContext.LeaveRules.FindAsync(id);
        }

        public void Update(LeaveRule entity)
        {
            _dataContext.LeaveRules.Update(entity);
        }
    }
}
