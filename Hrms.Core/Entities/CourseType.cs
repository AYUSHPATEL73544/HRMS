
namespace Hrms.Core.Entities
{
    public class CourseType : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<JobApplication> Candidates { get; set; }
    }
}
