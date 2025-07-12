
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Abstractions;
using Hrms.Core.Entities;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class AttendanceManager : IAttendanceManager

    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceManager(IAttendanceRepository attendanceRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AttendanceModel model)
        {
            var employeeIds = model.EmployeeIds;
            foreach (var employeeId in employeeIds)
            {
                var employee = await _employeeRepository.GetAsync(employeeId);
                var attendance = new Attendance
                {
                    EmployeeId = employee.Id,
                    Date = model.Date,
                    Status = Constants.RecordStatus.Active,
                    AttendanceLogs = new List<AttendanceLog>()
                };

                foreach (var attendanceLog in model.Logs)
                {
                    attendance.AttendanceLogs.Add(new AttendanceLog
                    {
                        AttendanceId = attendanceLog.AttendanceId,
                        InTime = attendanceLog.InTime,
                        OutTime = attendanceLog.OutTime,
                        Latitude = attendanceLog.Latitude,
                        Longitude = attendanceLog.Longitude
                    });
                }
                await _attendanceRepository.AddAsync(attendance);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AttendanceModel> GetAsync(int id)
        {
            return await _attendanceRepository.GetAsync(id);
        }

        public async Task<List<AttendanceModel>> GetListAsync()
        {
            return await _attendanceRepository.GetListAsync();
        }

        public async Task UpdateAsync(AttendanceModel model)
        {
            var attendance = await _attendanceRepository.FindAsync(model.Id);

            attendance.EmployeeId = model.EmployeeId;
            attendance.Date = model.Date;
            _attendanceRepository.Update(attendance);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _attendanceRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            _attendanceRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
