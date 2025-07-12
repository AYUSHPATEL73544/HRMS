using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Entities;
using Hrms.Core.Utilities;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Abstractions.Managers;
using System.Data;
using Hrms.Core.Models;

namespace Hrms.Core.Managers
{
    public class AttendanceRuleManager : IAttendanceRuleManager
    {
        private readonly IAttendanceRuleRepository _attendanceRuleRepository;
        private readonly IEmployeeAttendanceRuleRepository _employeeAttendanceRuleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceRuleManager(IAttendanceRuleRepository attendanceRuleRepository,
            IEmployeeAttendanceRuleRepository employeeAttendanceRuleRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _attendanceRuleRepository = attendanceRuleRepository;
            _employeeAttendanceRuleRepository = employeeAttendanceRuleRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            
        }

        public async Task AddAsync(AttendanceRuleModel model)
        {
            var effectiveFrom = new DateTime(model.Year, 1, 1);
            var effectiveTo = new DateTime(model.Year, 12, 31);
             
            if (model.InTime > model.GraceInTime)
            {
                throw new InvalidOperationException("Clock in time cannot be greater than grace in time.");
            }
            if (model.OutTime > model.GraceOutTime)
            {
                throw new InvalidOperationException("Clock out time cannot be greater than grace out time.");
            }
            else
            {
                var attedanceRule = new AttendanceRule
                {
                    CompanyId = model.CompanyId,
                    Title = model.Title,
                    Description = model.Description,
                    InTime = model.InTime,
                    OutTime = model.OutTime,
                    FirstHalfStart = model.FirstHalfStart,
                    FirstHalfEnd = model.FirstHalfEnd,
                    SecondHalfStart = model.SecondHalfStart,
                    SecondHalfEnd = model.SecondHalfEnd,
                    GraceInTime = model.GraceInTime,
                    GraceOutTime = model.GraceOutTime,
                    TotalBreakDuration = model.TotalBreakDuration,
                    MinEffectiveDuration = model.MinEffectiveDuration,
                    AutoLeaveDeduction = model.AutoLeaveDeduction,
                    MinAnomaliesForFistHalfDeduction = model.MinAnomaliesForFistHalfDeduction,
                    MinAnomaliesForFullDayDeduction = model.MinAnomaliesForFullDayDeduction,
                    Status = Constants.RecordStatus.Active,
                    StartDay = model.StartDay,
                    EndDay = model.EndDay,
                    NumberOfBreaks = model.NumberOfBreak,
                    EffectiveFrom = effectiveFrom,
                    EffectiveTo = effectiveTo
                   
                };
                await _attendanceRuleRepository.AddAsync(attedanceRule);

                if(model.ForwardToNextYear)
                {
                    var nextYearRule = new AttendanceRule
                    {
                        CompanyId = model.CompanyId,
                        Title = model.Title,
                        Description = model.Description,
                        InTime = model.InTime,
                        OutTime = model.OutTime,
                        FirstHalfStart = model.FirstHalfStart,
                        FirstHalfEnd = model.FirstHalfEnd,
                        SecondHalfStart = model.SecondHalfStart,
                        SecondHalfEnd = model.SecondHalfEnd,
                        GraceInTime = model.GraceInTime,
                        GraceOutTime = model.GraceOutTime,
                        TotalBreakDuration = model.TotalBreakDuration,
                        MinEffectiveDuration = model.MinEffectiveDuration,
                        AutoLeaveDeduction = model.AutoLeaveDeduction,
                        MinAnomaliesForFistHalfDeduction = model.MinAnomaliesForFistHalfDeduction,
                        MinAnomaliesForFullDayDeduction = model.MinAnomaliesForFullDayDeduction,
                        Status = Constants.RecordStatus.Active,
                        StartDay = model.StartDay,
                        EndDay = model.EndDay,
                        NumberOfBreaks = model.NumberOfBreak,
                        EffectiveFrom = effectiveFrom.AddYears(1),
                        EffectiveTo = effectiveTo.AddYears(1)
                    };
                    await _attendanceRuleRepository.AddAsync(nextYearRule);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<AttendanceRuleModel> GetAsync(int id)
        {
            return await _attendanceRuleRepository.GetAsync(id);
        }

        public async Task<MatTableResponse<AttendanceRuleModel>> GetPagedListAsync(MatDataTableRequest model, int userId)
        {
            var employee = await _employeeRepository.GetByUserIdAsync(userId);
            if(employee == null)
            {
                return await _attendanceRuleRepository.GetPagedListAsync(model);
            }
            else
            {
                return await _attendanceRuleRepository.GetPagedListAsync(model, employee.Id);
            }
        }

        public async Task<List<SelectListItemModel>> GetSelectListItemAsync()
        {
            return await _attendanceRuleRepository.GetSelectListItemAsync();
        }

        public async Task<MatTableResponse<AttendanceRuleListItemModel>> GetPagedResultAsync(MatDataTableRequest model)
        {
            return await _attendanceRuleRepository.GetPagedResultAsync(model);
        }

        public async Task UpdateAsync(AttendanceRuleModel model)
        {
            var attendanceRule = await _attendanceRuleRepository.FindAsync(model.Id);

            if (model.FormType == "timing")
            {

                if (model.InTime >= model.OutTime)
                {
                    throw new InvalidOperationException("In-time cannot be greater than or equal to out-time.");
                }
                if ((model.InTime >= model.GraceInTime) || (model.OutTime >= model.GraceOutTime))
                {
                    throw new InvalidOperationException("Actual time cannot be greater than grace time.");
                }
                if (model.GraceInTime >= model.OutTime)
                {
                    throw new InvalidOperationException("Grace in-time cannot be greater than out-time.");
                }
            }

            if (model.FormType == "workDuration")
            {
                if (model.FirstHalfStart >= model.FirstHalfEnd)
                {
                    throw new InvalidOperationException("First half start time cannot be greater than first half end time");
                }
                if ((model.FirstHalfStart >= model.SecondHalfStart) || (model.FirstHalfStart >= model.SecondHalfEnd))
                {
                    throw new InvalidOperationException("First half time cannot be greater than or equal to second half time.");
                }
                if (model.SecondHalfStart >= model.SecondHalfEnd)
                {
                    throw new InvalidOperationException("second half start time cannot be greater than second half end time");
                }
                if (model.FirstHalfEnd >= model.SecondHalfStart)
                {
                    throw new InvalidOperationException("First half start time cannot be greater than second half start time");
                }
            }
            
                attendanceRule.CompanyId = model.CompanyId;
                attendanceRule.Title = model.Title;
                attendanceRule.Description = model.Description;
                attendanceRule.InTime = model.InTime;
                attendanceRule.OutTime = model.OutTime;
                attendanceRule.FirstHalfStart = model.FirstHalfStart;
                attendanceRule.FirstHalfEnd = model.FirstHalfEnd;
                attendanceRule.SecondHalfStart = model.SecondHalfStart;
                attendanceRule.SecondHalfEnd = model.SecondHalfEnd;
                attendanceRule.GraceInTime = model.GraceInTime;
                attendanceRule.GraceOutTime = model.GraceOutTime;
                attendanceRule.TotalBreakDuration = model.TotalBreakDuration;
                attendanceRule.MinEffectiveDuration = model.MinEffectiveDuration;
                attendanceRule.AutoLeaveDeduction = model.AutoLeaveDeduction;
                attendanceRule.MinAnomaliesForFistHalfDeduction = model.MinAnomaliesForFistHalfDeduction;
                attendanceRule.MinAnomaliesForFullDayDeduction = model.MinAnomaliesForFullDayDeduction;
                attendanceRule.StartDay = model.StartDay;
                attendanceRule.EndDay = model.EndDay;
                attendanceRule.NumberOfBreaks = model.NumberOfBreak;

                _attendanceRuleRepository.Update(attendanceRule);
                await _unitOfWork.SaveChangesAsync();
            
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _attendanceRuleRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            _attendanceRuleRepository.Update(entity);

            var assignRules = await _employeeAttendanceRuleRepository.GetByRuleIdAsync(entity.Id);
            if(assignRules != null)
            {
                foreach(var assignRule in assignRules)
                {
                    assignRule.Status = Constants.RecordStatus.Deleted;
                    _employeeAttendanceRuleRepository.Update(assignRule);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AttendanceRuleModel> GetByYearAsync(int year)
        {
            return await _attendanceRuleRepository.GetByYearAsync(year);
        }

        public async Task<bool> IsExistsAsync(int year)
        {
            return await _attendanceRuleRepository.IsExistsAsync(year);
        }
    }
}
