

using Hrms.Core.Utilities;

namespace Hrms.Core.Models.JobApplication
{
    public class ApplicantModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QualificationTypeId { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseName { get; set; }
        public string Stream { get; set; }
        public int PassingYear { get; set; }
        public int Gender { get; set; }
        public bool IsHired { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public bool IsPursuing { get; set; }
        public List<int> SkillIds { get; set; }
        public List<string>SkillNames { get; set; }
        public Constants.InterviewMode InterviewMode { get; set; }
        public Constants.InterviewType InterviewType { get; set; }

        public string Remark { get; set; }
        public bool IsShortlisted { get; set; }
        public string InterviewerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan? SchedulerTime { get; set; }
        public Constants.RecordStatus InterviewStatus { get; set; }
        public DateTime? ShortlistedDate { get; set; }
        public FileDetailModel DocumentDetails { get; set; }
        public string MarketingChannel { get; set; }


    }
}
