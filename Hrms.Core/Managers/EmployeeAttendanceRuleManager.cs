using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Entities;
using Hrms.Core.Utilities;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Models;

namespace Hrms.Core.Managers
{
    public class EmployeeAttendanceRuleManager : IEmployeeAttendanceRuleManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeAttendanceRuleRepository _employeeAttendanceRuleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAttendanceRuleRepository _attendanceRuleRepository;

        public EmployeeAttendanceRuleManager(IUnitOfWork unitOfWork,
            IEmployeeAttendanceRuleRepository employeeAttendanceRuleRepository,
            IEmployeeRepository employeeRepository,
            IAttendanceRuleRepository attendanceRuleRepository)
        {
            _unitOfWork = unitOfWork;
            _employeeAttendanceRuleRepository = employeeAttendanceRuleRepository;
            _employeeRepository = employeeRepository;
            _attendanceRuleRepository = attendanceRuleRepository;
        }

        public async Task AddAsync(EmployeeAttendanceModel model)
        {
                foreach (var employeeId in model.EmployeeIds)
                {
                    if(await _employeeAttendanceRuleRepository.IsExistAsync(employeeId))
                    {
                        var entity = await _employeeAttendanceRuleRepository.GetByEmployeeIdAsync(employeeId);
                        entity.AttendanceRuleId = model.RuleId;
                        entity.CreatedOn = Utility.GetDateTime();
                        entity.EffectiveFrom = Utility.GetDateTime();

                        _employeeAttendanceRuleRepository.Update(entity);

                    }
                    else { 
                        var rule = await _attendanceRuleRepository.GetAsync(model.RuleId);
                        var employee = await _employeeRepository.GetAsync(employeeId);
                        var attendanceRule = new EmployeeAttendanceRule
                        {
                            AttendanceRuleId = rule.Id,
                            EmployeeId = employee.Id,
                            Status = Constants.RecordStatus.Active,
                            CreatedOn = Utility.GetDateTime(),
                            EffectiveFrom = Utility.GetDateTime()
                        };
                        await _employeeAttendanceRuleRepository.AddAsync(attendanceRule);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

        }

        public async Task<MatTableResponse<EmployeeAttendanceModel>> GetpagedListAsync(MatDataTableRequest model)
        {
            return await _employeeAttendanceRuleRepository.GetpagedListAsync(model);
        }

        public async Task<MatTableResponse<EmployeeAttendanceModel>> GetInActivepagedListAsync(MatDataTableRequest model)
        {
            return await _employeeAttendanceRuleRepository.GetInActivepagedListAsync(model);
        }
    }
}
