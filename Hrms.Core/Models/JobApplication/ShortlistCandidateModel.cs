using Hrms.Core.Utilities;


namespace Hrms.Core.Models.JobApplication
{
    public class ShortlistCandidateModel
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string LegalName { get; set; }
        public DateTime? ShortlistedDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public TimeSpan? ScheduleTime { get; set; }
        public Constants.RecordStatus Status { get; set; }

        public int? InterviewId { get; set; }
    }
}
