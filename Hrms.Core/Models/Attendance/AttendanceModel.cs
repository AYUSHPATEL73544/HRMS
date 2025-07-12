
using Hrms.Core.Entities;
using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Attendance
{
    public class AttendanceModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; init; }

        public TimeSpan? WorkDuration { get; set; }
        public TimeSpan? FirstClockIn { get; set; }
        public TimeSpan? LastClockOut {get;set;}
        public bool IsWorkDurationLess { get; set; }
        public bool IsLateClockIn { get; set; }
        public List<AttendanceLogModel> Logs { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public AttendanceRule AttendanceRule { get; set; }
        public List<int> EmployeeIds { get; set; }
        public List<int> RuleIds { get; set; }
    }
}
