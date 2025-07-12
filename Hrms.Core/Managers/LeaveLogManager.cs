using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;
using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace Hrms.Core.Managers
{
    public class LeaveLogManager : ILeaveLogManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveLogRepository _leaveLogRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IHolidayRepository _holidayRepository;
        private readonly ILeaveRuleRepository _leaveRuleRepository;

        public LeaveLogManager(ILeaveLogRepository leaveLogRepository,
            IEmployeeRepository employeeRepository,
            ILeaveRepository leaveRepository,
            ILeaveRuleRepository leaveRuleRepository,
            IHolidayRepository holidayRepository,
            IUnitOfWork unitOfWork)
        {
            _leaveLogRepository = leaveLogRepository;
            _employeeRepository = employeeRepository;
            _leaveRepository = leaveRepository;
            _leaveRuleRepository = leaveRuleRepository;
            _holidayRepository = holidayRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsExistsAsync(int userId, DateTime startDate, DateTime endDate, int id = 0)
        { 
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
            return await _leaveLogRepository.IsExistsAsync(employeeId, startDate, endDate, id);
        }

        public async Task AddAsync(LeaveLogModel model, int userId)
        {

            try
            {
                var currentDate = Utility.GetDateTime(); 

                await _unitOfWork.BeginTransactionAsync();

                var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);

                var leave = await _leaveRepository.GetByRuleIdAsync(employeeId, model.RuleId);
                var leaveRule = await _leaveRuleRepository.GetAsync(model.RuleId);

                if (leaveRule.AllowedCarryForward && model.EndDate.Year > currentDate.Year)
                {
                    throw new InvalidOperationException($"You can apply leave only for this year.");
                }

                var monthlyLeaveLogs = await _leaveLogRepository.GetMonthlyLeaveLogAsync(employeeId, model.RuleId, model.StartDate);

                var holidays = await _holidayRepository.GetListAsync(model.StartDate.Year, model.StartDate.Month);
                var leavesApplied = CalculateAppliedLeaveCount(model, leaveRule.CountWeekendAsLeave, leaveRule.CountHolidayAsLeave, holidays);

                if (leavesApplied > leave.Available)
                {
                    throw new InvalidOperationException("You don't have enough leaves available.");
                }
                if (!leaveRule.CountWeekendAsLeave)
                {
                    if (leavesApplied == 0)
                    {
                        throw new InvalidOperationException("Weekends or holidays not eligible. Choose different working days.");
                    }
                    for (DateTime day = model.StartDate; day <= model.EndDate; day = day.AddDays(1))
                    {
                        if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                        {
                            throw new InvalidOperationException("Invalid Start Date: Weekend not eligible. Choose different working day.");
                        }
                    }
                }
                else if (leaveRule.CountWeekendAsLeave && !leaveRule.CountHolidayAsLeave && model.StartDate.DayOfWeek == DayOfWeek.Monday)
                {
                    var previousLeaveLog = await _leaveLogRepository.PreviousLeaveLogAsync(model.StartDate, employeeId, model.RuleId);

                    if (previousLeaveLog != null)
                    {
                        leavesApplied += 2;
                        previousLeaveLog.Days = previousLeaveLog.Days + leavesApplied + model.Days;
                        previousLeaveLog.EndDate = model.EndDate;
                        previousLeaveLog.Status = Constants.RecordStatus.Pending;

                        _leaveLogRepository.Update(previousLeaveLog);

                        leave.Applied += leavesApplied;
                        leave.Available -= leavesApplied;

                        _leaveRepository.Update(leave);

                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();
                        return;
                    }
                }
                else if (leaveRule.CountHolidayAsLeave && leaveRule.CountWeekendAsLeave && model.StartDate.DayOfWeek == DayOfWeek.Monday)
                {
                    var totalHolidayCount = 0;
                    var previousLeave = await _leaveLogRepository.GetPreviousLeaveLogDatesAsync(model.StartDate, employeeId, model.RuleId);
                    if (previousLeave != null)
                    {

                        totalHolidayCount = await CalculateTotalHolidayCount(previousLeave.EndDate, model.StartDate);
                        if (totalHolidayCount > 0)
                        {
                            leavesApplied += totalHolidayCount;
                            previousLeave.Days = previousLeave.Days + leavesApplied + model.Days;
                            previousLeave.EndDate = model.EndDate;
                            previousLeave.Status = Constants.RecordStatus.Pending;

                            _leaveLogRepository.Update(previousLeave);

                            leave.Applied += leavesApplied;
                            leave.Available -= leavesApplied;

                            _leaveRepository.Update(leave);

                            await _unitOfWork.SaveChangesAsync();
                            await _unitOfWork.CommitAsync();
                            return;
                        }
                    }
                }
                else if (leaveRule.CountHolidayAsLeave)
                {
                    var totalHolidayCount = 0;
                    var previousLeave = await _leaveLogRepository.GetPreviousLeaveLogDatesAsync(model.StartDate, employeeId, model.RuleId);
                    if (previousLeave != null)
                    {
                            totalHolidayCount = await CalculateTotalHolidayCount(previousLeave.EndDate, model.StartDate);
                            if (totalHolidayCount > 0)
                            {
                            leavesApplied += totalHolidayCount;
                            previousLeave.Days = previousLeave.Days + leavesApplied + model.Days;
                            previousLeave.EndDate = model.EndDate;
                            previousLeave.Status = Constants.RecordStatus.Pending;

                            _leaveLogRepository.Update(previousLeave);

                            leave.Applied += leavesApplied;
                            leave.Available -= leavesApplied;

                            _leaveRepository.Update(leave);

                            await _unitOfWork.SaveChangesAsync();
                            await _unitOfWork.CommitAsync();
                            return;
                        }
                    }
                }

                if (model.StartDate.Date < currentDate.Date)
                {
                    var applyBackedLeaveUpTo = currentDate.AddDays(-leaveRule.MaxBackDatedLeavesAllowed);
                    if (leaveRule.AllowedBackDatedLeaves)
                    {
                        if (applyBackedLeaveUpTo > model.StartDate.Date)
                        {
                            string date = applyBackedLeaveUpTo.Date.ToShortDateString();
                            throw new InvalidOperationException($"Backdated leaves are acceptable up to {date}.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Backdated leaves are not allowed.");
                    }
                }


                if (model.StartDate.Date > currentDate.Date)
                {
                    var applyFutureLeaveUpTo = currentDate.AddDays(+leaveRule.FutureDatedLeavesAllowedUpTo);
                    if (leaveRule.FutureDatedLeavesAllowed)
                    {
                        if (applyFutureLeaveUpTo < model.EndDate.Date)
                        {
                            string date = applyFutureLeaveUpTo.Date.ToShortDateString();
                            throw new InvalidOperationException($"Futuredated leaves are acceptable up to {date}.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Futuredated leaves are not allowed.");
                    }
                }


                if (leaveRule.MaxAllowedContinues < leavesApplied)
                {
                    throw new InvalidOperationException($"You can't apply more than {leaveRule.MaxAllowedContinues} leave in continues.");
                }

                if (leaveRule.MaxAllowedInMonth < leavesApplied || monthlyLeaveLogs >= leaveRule.MaxAllowedInMonth)
                {
                    throw new InvalidOperationException($"You can't apply more than {leaveRule.MaxAllowedInMonth} leave in a month.");
                }

                var leaveLog = new LeaveLog
                {
                    EmployeeId = employeeId,
                    RuleId = model.RuleId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    StartHalf = model.StartHalf,
                    EndHalf = model.EndHalf,
                    CreatedOn = Utility.GetDateTime(),
                    EffectiveFrom = Utility.GetDateTime(),
                    CreatedById = userId,
                    Purpose = model.Purpose,
                    Status = Constants.RecordStatus.Pending,
                    Days = leavesApplied
                };

                await _leaveLogRepository.AddAsync(leaveLog);
                leave.Applied += leavesApplied;
                leave.Available -= leavesApplied;

                _leaveRepository.Update(leave);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<LeaveLogModel> GetAsync(int id)
        {
            return await _leaveLogRepository.GetAsync(id);
        }

        public async Task<MatTableResponse<LeaveLogModel>> PagedListByUserIdAsync(LeaveLogFilterModel model, int userId)
        {
            var employeeId = await _employeeRepository.GetByUserIdAsync(userId);
            return await _leaveLogRepository.PagedListByEmployeeIdAsync(model, employeeId.Id);
        }

        public async Task<MatTableResponse<LeaveLogModel>> PagedListByEmployeeIdAsync(LeaveLogFilterModel model, int employeeId)
        {
            return await _leaveLogRepository.PagedListByEmployeeIdAsync(model, employeeId);
        }

        public async Task<MatTableResponse<LeaveLogModel>> GetPagedListAsync(LeaveLogFilterModel model)
        {
            return await _leaveLogRepository.GetPagedListAsync(model);
        }

        public async Task<MatTableResponse<LeaveLogModel>> GetReporteePagedListAsync(LeaveLogFilterModel model, int userId)
        {
            return await _leaveLogRepository.GetReporteePagedListAsync(model, userId);
        }

        public async Task<MatTableResponse<LeaveLogModel>> GetPendingLeavesPagedListAsync(LeaveLogFilterModel model)
        {
            return await _leaveLogRepository.GetPendingLeavesPagedListAsync(model);
        }

        public async Task<int> GetTotalLeaveCountAsync(LeaveLogModel model, int userId)
        {
            TimeSpan difference = new TimeSpan();
            var totalHolidayCount = 0;

            var leaveRule = await _leaveRuleRepository.GetAsync(model.RuleId);
            if (leaveRule.CountWeekendAsLeave && !leaveRule.CountHolidayAsLeave && model.StartDate.DayOfWeek == DayOfWeek.Monday)
            {
                var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
                var previousLeave = await _leaveLogRepository.PreviousLeaveLogAsync(model.StartDate, employeeId, model.RuleId);
                if (previousLeave != null)
                {
                    difference = model.EndDate - previousLeave.StartDate;
                }
                else
                {
                    difference = model.EndDate - model.StartDate;
                }
            }
            else if (leaveRule.CountHolidayAsLeave && leaveRule.CountWeekendAsLeave && model.StartDate.DayOfWeek == DayOfWeek.Monday)
            {
                var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
                var previousLeave = await _leaveLogRepository.GetPreviousLeaveLogDatesAsync(model.StartDate, employeeId, model.RuleId);
                if (previousLeave != null)
                {
                    totalHolidayCount = await CalculateTotalHolidayCount(previousLeave.EndDate, model.StartDate);
                    if (totalHolidayCount > 0)
                    {
                        difference = model.EndDate - previousLeave.StartDate;
                    }
                    else
                    {
                        difference = model.EndDate - model.StartDate;
                    }
                }
                else
                {
                    difference = model.EndDate - model.StartDate;
                }
            }

            else if (leaveRule.CountHolidayAsLeave)
            {
                var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
                var previousLeave = await _leaveLogRepository.GetPreviousLeaveLogDatesAsync(model.StartDate, employeeId, model.RuleId);
                if (previousLeave != null)
                {
                    totalHolidayCount = await CalculateTotalHolidayCount(previousLeave.EndDate, model.StartDate);
                    if (totalHolidayCount > 0)
                    {
                        difference = model.EndDate - previousLeave.StartDate;
                    }
                    else
                    {
                        difference = model.EndDate - model.StartDate;
                    }
                }
                else
                {
                    difference = model.EndDate - model.StartDate;
                }
            }
            else
            {
                difference = model.EndDate - model.StartDate;
            }
            
            var totalDays = difference.Days + 1;
            return totalDays;
        }

        public async Task UpdateAsync(LeaveLogModel model)
        {
            var currentDate = Utility.GetDateTime();
            var leaveLog = await _leaveLogRepository.FindAsync(model.Id);
            var leave = await _leaveRepository.GetByRuleIdAsync(model.EmployeeId, model.RuleId);
            var reduceDays = leaveLog.Days;
            if (reduceDays >= 0)
            {
                leave.Applied -= reduceDays;
                leave.Available += reduceDays;
            }
            var leaveRule = await _leaveRuleRepository.GetAsync(model.RuleId);
            var holidays = await _holidayRepository.GetListAsync(model.StartDate.Year, model.StartDate.Month);
            var monthlyLeaveLogs = await _leaveLogRepository.GetMonthlyLeaveLogAsync(model.EmployeeId, model.RuleId, model.StartDate);
            var leavesApplied = CalculateAppliedLeaveCount(model, leaveRule.CountWeekendAsLeave, leaveRule.CountHolidayAsLeave, holidays);

            var isExists = await _leaveLogRepository.IsExistsAsync(model.EmployeeId, model.StartDate, model.EndDate, model.Id);
            if (isExists)
            {
                throw new InvalidOperationException("You have already applied for leave during the selected date range.");
            }
            if (leavesApplied > leave.Available)
            {
                throw new InvalidOperationException("You don't have enough leaves available.");
            }

            if (!leaveRule.CountWeekendAsLeave)
            {
                if (leavesApplied == 0)
                {
                    throw new InvalidOperationException("Weekends or holidays not eligible. Choose different working days.");
                }
                for (DateTime day = model.StartDate; day <= model.EndDate; day = day.AddDays(1))
                {
                    if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                    {
                        throw new InvalidOperationException("Invalid Start Date: Weekend not eligible. Choose different working day.");
                    }
                }
            }
            else if (leaveRule.CountWeekendAsLeave && !leaveRule.CountHolidayAsLeave && model.StartDate.DayOfWeek == DayOfWeek.Monday)
            {
                var previousLeave = await _leaveLogRepository.PreviousLeaveLogAsync(model.StartDate, model.EmployeeId, model.RuleId);
                if (previousLeave != null)
                {
                    model.StartDate = previousLeave.StartDate;

                    leave.Applied -= previousLeave.Days;
                    leave.Available += previousLeave.Days;

                    TimeSpan difference = model.EndDate - model.StartDate;
                    int totalDays = difference.Days + 1;

                    leavesApplied = totalDays;

                    previousLeave.Status = Constants.RecordStatus.Deleted;
                    _leaveLogRepository.Update(previousLeave);
                }
            }
            else if (leaveRule.CountHolidayAsLeave && leaveRule.CountWeekendAsLeave && model.StartDate.DayOfWeek == DayOfWeek.Monday)
            {
                var previousLeave = await _leaveLogRepository.GetPreviousLeaveLogDatesAsync(model.StartDate, model.EmployeeId, model.RuleId);
                if (previousLeave != null)
                {
                    model.StartDate = previousLeave.StartDate;

                    leave.Applied -= previousLeave.Days;
                    leave.Available += previousLeave.Days;

                    TimeSpan difference = model.EndDate - model.StartDate;
                    int totalDays = difference.Days + 1;

                    leavesApplied = totalDays;

                    previousLeave.Status = Constants.RecordStatus.Deleted;
                    _leaveLogRepository.Update(previousLeave);
                }
            }

            else if (leaveRule.CountHolidayAsLeave)
            {
                var previousLeave = await _leaveLogRepository.GetPreviousLeaveLogDatesAsync(model.StartDate, model.EmployeeId, model.RuleId);
                if (previousLeave != null)
                {
                    model.StartDate = previousLeave.StartDate;

                    leave.Applied -= previousLeave.Days;
                    leave.Available += previousLeave.Days;

                    TimeSpan difference = model.EndDate - model.StartDate;
                    int totalDays = difference.Days + 1;

                    leavesApplied = totalDays;

                    previousLeave.Status = Constants.RecordStatus.Deleted;
                    _leaveLogRepository.Update(previousLeave);
                }
            }


            if (model.StartDate.Date <= currentDate.Date || model.StartDate.Date >= currentDate.Date)
            {
                var applyBackedLeaveUpTo = currentDate.AddDays(-leaveRule.MaxBackDatedLeavesAllowed);
                if (leaveRule.AllowedBackDatedLeaves)
                {
                    if (applyBackedLeaveUpTo > model.StartDate.Date)
                    {
                        string date = applyBackedLeaveUpTo.Date.ToShortDateString();
                        throw new InvalidOperationException($"Backdated leaves are acceptable up to {date}.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Backdated leaves are not allowed.");
                }

                var applyFutureLeaveUpTo = currentDate.AddDays(+leaveRule.FutureDatedLeavesAllowedUpTo);
                if (leaveRule.FutureDatedLeavesAllowed && (applyFutureLeaveUpTo < model.EndDate.Date))
                {
                    string date = applyFutureLeaveUpTo.Date.ToShortDateString();
                    throw new InvalidOperationException($"Futuredated leaves are acceptable up to {date}.");
                }
            }

            if (leaveRule.MaxAllowedContinues < leavesApplied)
            {
                throw new InvalidOperationException($"You can't apply more than {leaveRule.MaxAllowedContinues} leave in continues.");
            }

            if (leaveRule.MaxAllowedInMonth < leavesApplied || monthlyLeaveLogs >= leaveRule.MaxAllowedInMonth)
            {
                throw new InvalidOperationException($"You can't apply more than {leaveRule.MaxAllowedInMonth} leave in a month.");
            }

            leaveLog.EmployeeId = model.EmployeeId;
            leaveLog.RuleId = model.RuleId;
            leaveLog.StartDate = model.StartDate;
            leaveLog.EndDate = model.EndDate;
            leaveLog.StartHalf = model.StartHalf;
            leaveLog.EndHalf = model.EndHalf;
            leaveLog.Status = Constants.RecordStatus.Pending;
            leaveLog.Days = leavesApplied;

            _leaveLogRepository.Update(leaveLog);

            leave.Applied += leavesApplied;
            leave.Available -= leavesApplied;


            _leaveRepository.Update(leave);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(LeaveLogChangeStatusModel model)
        {
            var leaveLog = await _leaveLogRepository.FindAsync(model.Id);
            if (model.Status == Constants.RecordStatus.Rejected)
            {
                leaveLog.RejectionReason = model.RejectionReason;
            }
            leaveLog.Status = model.Status;
            _leaveLogRepository.Update(leaveLog);

            var leave = await _leaveRepository.GetByRuleIdAsync(leaveLog.EmployeeId, leaveLog.RuleId);
            if (model.Status == Constants.RecordStatus.Rejected)
            {
                leave.Available += leaveLog.Days;
                leave.Applied -= leaveLog.Days;
            }
            _leaveRepository.Update(leave);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var entity = await _leaveLogRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            _leaveLogRepository.Update(entity);

            var leave = await _leaveRepository.GetByRuleIdAsync(entity.EmployeeId, entity.RuleId);

            leave.Available += entity.Days;
            leave.Applied -= entity.Days;

            _leaveRepository.Update(leave);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<List<LeaveLogModel>> GetLeaveLog(DateTime startDate, DateTime endDate)
        {
            return await _leaveLogRepository.GetLeaveLog(startDate,endDate);
        }
        
        #region private

        private decimal CalculateAppliedLeaveCount(LeaveLogModel model, bool countWeekEndAsLeave, bool countHolidayAsLeave, List<HolidayModel> holidays)
        {
            if ((int)(model.EndDate.Date - model.StartDate.Date).TotalDays == 0)
            {
                if (model.StartHalf != 0 && model.StartHalf == model.EndHalf)
                {
                    return 0.5m;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                int totalDays = (int)(model.EndDate - model.StartDate).TotalDays + 1;
                int weekends = 0;
                int holidaysCount = 0;

                if (!countWeekEndAsLeave || !countHolidayAsLeave)
                {
                    for (DateTime date = model.StartDate; date <= model.EndDate; date = date.AddDays(1))
                    {
                        if (!countWeekEndAsLeave && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                        {
                            weekends++;
                        }

                        if (!countHolidayAsLeave && holidays.Any(h => h.Date.Date == date.Date))
                        {
                            holidaysCount++;
                        }
                    }
                }

                return totalDays - weekends - holidaysCount;
            }
        }

       
        private async Task<int> CalculateTotalHolidayCount(DateTime lastLeaveEndDate, DateTime newLeaveStartDate)
        {
            int holidayCount = 0;
            for (DateTime date = lastLeaveEndDate.AddDays(1); date < newLeaveStartDate; date = date.AddDays(1))
            {
                if (await _holidayRepository.IsHolidayExistAsync(date))
                {
                    holidayCount++;
                }
                else if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    holidayCount++;
                }
                else
                {
                    holidayCount = 0;
                    break;
                }
            }
            //int totalLeaveCount = (newLeaveStartDate - lastLeaveEndDate).Days + leaveCount;
            return holidayCount;
        }

    }
        #endregion
    }

