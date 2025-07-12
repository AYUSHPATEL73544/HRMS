using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Attendance
{
    public class AttendanceLogModel
    {
        public int Id { get; set; }
        public int AttendanceId { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan GraceInTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public string Note { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Date { get; set; }
        public decimal WorkDuration { get; set; }
        public Constants.RecordStatus EmployeeStatus { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsWorkDurationLess { get; set; }
        public bool IsLateClockIn { get; set; }

    }
}
