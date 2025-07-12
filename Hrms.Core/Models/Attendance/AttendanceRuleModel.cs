using Hrms.Core.Utilities;

namespace Hrms.Core.Models.Attendance
{
    public class AttendanceRuleModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public TimeSpan FirstHalfStart { get; set; }
        public TimeSpan FirstHalfEnd { get; set; }
        public TimeSpan SecondHalfStart { get; set; }
        public TimeSpan SecondHalfEnd { get; set; }
        public TimeSpan GraceInTime { get; set; }
        public TimeSpan GraceOutTime { get; set; }
        public TimeSpan TotalBreakDuration { get; set; }
        public TimeSpan MinEffectiveDuration { get; set; }
        public bool AutoLeaveDeduction { get; set; }
        public int MinAnomaliesForFistHalfDeduction { get; set; }
        public int MinAnomaliesForFullDayDeduction { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public int NumberOfBreak { get; set; }
        public string FormType { get; set; }
        public int Peoples { get; set; }
        public int Year { get; set; }
        public bool ForwardToNextYear { get; set; }

    }
}
