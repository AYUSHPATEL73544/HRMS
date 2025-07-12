using Hrms.Core.Utilities;

namespace Hrms.Core.Entities
{
    public class Interview : EntityBase<int>
    {
        public Constants.InterviewMode InterviewMode { get; set; }
        public Constants.InterviewType InterviewType { get; set; }  
        public int CandidateId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public TimeSpan? ScheduleTime { get; set; }
        public int? Rating { get; set; }
        public bool? EligibleForNextRound { get; set; }
        public string Remark { get; set; }
        public int InterviewerId { get; set; }
        public virtual User User { get; set; }
    }
}
