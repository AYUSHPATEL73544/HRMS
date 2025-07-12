namespace Hrms.Core.Entities
{
    public class AttendanceRule :  EntityBase<int> // EntityWithStatusTracking<int>
    {
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; } 
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
        public int NumberOfBreaks { get; set; }
    }
}
