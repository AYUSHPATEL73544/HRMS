namespace Hrms.Core.Entities
{
    public class AttendanceLog : EntityBase<int>
    {
        public int AttendanceId { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; }

    }
}
