using Hrms.Core.Utilities;

namespace Hrms.Core.Models.JobApplication
{
    public class InterviewModel
    {
        public int Id { get; set; }
        public Constants.InterviewType InterviewType { get; set; }
        public Constants.InterviewMode InterviewMode { get; set; }
        public int CandidateId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public TimeSpan? ScheduleTime { get; set; }
        public int? Rating { get; set; }
        public bool? EligibleForNextRound { get; set; }
        public string InterviewerName { get; set; }
        public string Remark { get; set; }
        public int InterviewerId { get; set; }
        public string LegalName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Constants.RecordStatus Status { get;set; }
        public FileDetailModel DocumentDetails { get; set; }
    }
}
