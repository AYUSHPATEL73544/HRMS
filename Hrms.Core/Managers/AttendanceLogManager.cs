using Hrms.Core.Abstractions;
using Hrms.Core.Abstractions.Managers;
using Hrms.Core.Abstractions.Repositories;
using Hrms.Core.Entities;
using Hrms.Core.Models;
using Hrms.Core.Models.Attendance;
using Hrms.Core.Models.Leave;
using Hrms.Core.Utilities;

namespace Hrms.Core.Managers
{
    public class AttendanceLogManager : IAttendanceLogManager
    {
        private readonly IAttendanceLogRepository _attendanceLogRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeManagerRepository _employeeManagerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceLogManager(IAttendanceLogRepository attendanceLogRepository,
            IAttendanceRepository attendanceRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeManagerRepository employeeManagerRepository,
            IUnitOfWork unitOfWork)
        {
            _attendanceLogRepository = attendanceLogRepository;
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _employeeManagerRepository = employeeManagerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AttendanceLogModel model, int userId)
        {
            var attendanceId = await _attendanceRepository.GetByEmployeeIdAsync(model.EmployeeId, model.Date);

            if (attendanceId == null)
            {
                var attendance = new Attendance
                {
                    EmployeeId = model.EmployeeId,
                    Date = model.Date,
                    Status = Constants.RecordStatus.Active
                };
                await _attendanceRepository.AddAsync(attendance);
                await _unitOfWork.SaveChangesAsync();

                if (model.InTime > model.OutTime)
                {
                    throw new InvalidOperationException("Clock-in time cannot be greater than Clock-out time.");
                }
                var attedanceLog = new AttendanceLog
                {
                    AttendanceId = attendance.Id,
                    InTime = model.InTime,
                    OutTime = model.OutTime,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Status = Constants.RecordStatus.Active,
                    CreatedById = userId,
                    CreatedOn = Utility.GetDateTime(),
                    EffectiveFrom = Utility.GetDateTime(),
                    Note = model.Note,
                };
                await _attendanceLogRepository.AddAsync(attedanceLog);
            }
            else
            {
                var attedanceLog = new AttendanceLog
                {
                    AttendanceId = attendanceId.Id,
                    InTime = model.InTime,
                    OutTime = model.OutTime,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Status = Constants.RecordStatus.Active,
                    CreatedById = userId,
                    CreatedOn = Utility.GetDateTime(),
                    EffectiveFrom = Utility.GetDateTime()
                };
                await _attendanceLogRepository.AddAsync(attedanceLog);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClockInAsync(AttendanceLogModel model, int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);

            var attendanceId = await _attendanceRepository.GetByEmployeeIdAsync(employee, model.Date);

            if (attendanceId == null)
            {
                var attendance = new Attendance
                {
                    EmployeeId = employee,
                    Date = Utility.GetDateTime().Date,
                    Status = Constants.RecordStatus.Active
                };
                await _attendanceRepository.AddAsync(attendance);
                await _unitOfWork.SaveChangesAsync();

                var attedanceLog = new AttendanceLog
                {
                    AttendanceId = attendance.Id,
                    InTime = Utility.GetDateTime().TimeOfDay,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Status = Constants.RecordStatus.Active
                };
                await _attendanceLogRepository.AddAsync(attedanceLog);
            }
            else
            {
                var attedanceLog = new AttendanceLog
                {
                    AttendanceId = attendanceId.Id,
                    InTime = Utility.GetDateTime().TimeOfDay,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Status = Constants.RecordStatus.Active
                };
                await _attendanceLogRepository.AddAsync(attedanceLog);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClockOutAsync(AttendanceLogModel model, int userId)
        {
            var employee = await _employeeRepository.GetIdByUserIdAsync(userId);

            var attendanceId = await _attendanceRepository.GetByEmployeeIdAsync(employee, model.Date);

            var attendanceLog = await _attendanceLogRepository.FindByAttendanceAsync(attendanceId.Id);

            attendanceLog.OutTime = Utility.GetDateTime().TimeOfDay;

            _attendanceLogRepository.Update(attendanceLog);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AttendanceLogModel> GetAsync(int id)
        {
            return await _attendanceLogRepository.GetAsync(id);
        }

        public async Task<List<AttendanceLogModel>> GetAttendanceDeatilByEmployeeId(int id)
        {
            return await _attendanceLogRepository.GetAttendanceDeatilByEmployeeId(id);
        }

        public async Task<List<AttendanceModel>> GetListAsync()
        {
            return await _attendanceLogRepository.GetListAsync();
        }

        public async Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryByEmployeeId(AttedanceFilterModel model,int id, int userId)
        {
            return await _attendanceLogRepository.GetAttendanceHistoryByEmployeeId(model,id, userId);
        }

        public async Task<MatTableResponse<AttendanceLogModel>> GetEmployeeAttendanceHistoryAsync(AttedanceFilterModel model, int userId)
        {
            return await _attendanceLogRepository.GetEmployeeAttendanceHistoryAsync(model, userId);
        }

        public async Task<MatTableResponse<AttendanceLogModel>> GetAttendancDetailsForEmployeeAsync(AttedanceFilterModel model, int attendanceId)
        {
            return await _attendanceLogRepository.GetAttendancDetailsForEmployeeAsync(model, attendanceId);
        }

        public async Task<AttendanceLogModel> GetDetailAsync(int userId)
        {
            return await _attendanceLogRepository.GetDetailAsync(userId);
        }

        public async Task<MatTableResponse<AttendanceLogModel>> GetAttendanceHistoryAsync(AttedanceFilterModel model)
        {
            return await _attendanceLogRepository.GetAttendanceHistoryAsync(model);
        }

        public async Task UpdateAsync(AttendanceLogModel model, int userId)
        {
            var attendanceLog = await _attendanceLogRepository.FindAsync(model.Id);
            if (model.InTime > model.OutTime)
            {
                throw new InvalidOperationException("Intime cannot be greater than Outtime.");
            }
            attendanceLog.InTime = model.InTime;
            attendanceLog.OutTime = model.OutTime;
            attendanceLog.UpdatedById = userId;
            attendanceLog.CreatedOn = Utility.GetDateTime();
            attendanceLog.EffectiveFrom = Utility.GetDateTime();
            attendanceLog.CreatedById = userId;
            attendanceLog.Note = model.Note;

            _attendanceLogRepository.Update(attendanceLog);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAttendanceLogAsync(int id)
        {
            var entity = await _attendanceLogRepository.FindAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;
            _attendanceLogRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var entity = await _attendanceRepository.GetByAsync(id);

            entity.Status = Constants.RecordStatus.Deleted;

            foreach (var log in entity.AttendanceLogs)
            {
                log.Status = Constants.RecordStatus.Deleted;
            }

            _attendanceRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<MatTableResponse<AttendanceModel>> GetPagedListAsync(MatDataTableRequest model)
        {
            return await _attendanceLogRepository.GetPagedListAsync(model);
        }

        public async Task<MatTableResponse<AttendanceModel>> GetPagedListForEmployeeAsync(MatDataTableRequest model, int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);

            var employeeIds = await _employeeManagerRepository.GetEmployeeIdByManagerId(employeeId);

            return await _attendanceLogRepository.GetPagedListForEmployeeAsync(model, employeeIds, userId);
        }

        public async Task<List<AttendanceEventModel>> GetAttendanceLogEventAsync(int year, int month, int employeeId)
        {
            return await _attendanceLogRepository.GetAttendanceLogEventAsync(year, month, employeeId);
        }

        
        public async Task<List<AttendanceEventModel>> GetEmployeeAttendanceLogEventAsync(int year, int month, int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
            return await _attendanceLogRepository.GetAttendanceLogEventAsync(year, month, employeeId);
        }

        public async Task<List<AttendanceEventModel>> GetAbsentEventAsync(int year, int month, int employeeId)
        {
            return await _attendanceLogRepository.GetAbsentEventAsync(year, month, employeeId);
        }

        public async Task<List<AttendanceEventModel>> GetEmployeeAbsentEventAsync(int year, int month, int userId)
        {
            var employeeId = await _employeeRepository.GetIdByUserIdAsync(userId);
            return await _attendanceLogRepository.GetAbsentEventAsync(year, month, employeeId);
        }

    }
}
