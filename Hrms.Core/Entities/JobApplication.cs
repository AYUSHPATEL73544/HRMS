namespace Hrms.Core.Entities
{
    public class JobApplication : EntityBase<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int QualificationTypeId { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseName { get; set; }
        public string Stream { get; set; }
        public int PassingYear { get; set; }
        public int Gender { get; set; }
        public bool Pursuing { get; set; }
        public string Remark { get; set; }
        public bool Shortlisted { get; set; }
        public bool Hired { get; set; }
        public DateTime? ShortlistedDate { get; set; }
        public virtual ICollection<ApplicantsSkill> ApplicantsSkills { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public string MarketingChannel { get; set; }

    }
}
