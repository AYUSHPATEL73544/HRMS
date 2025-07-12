
namespace Hrms.Core.Entities
{
    public class QualificationType : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<JobApplication> Candidates { get; set; }
    }
}
