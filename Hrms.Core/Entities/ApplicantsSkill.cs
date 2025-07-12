namespace Hrms.Core.Entities
{
    public class ApplicantsSkill : Entity<int>
    {
        public int ApplicantId { get; set; }
        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
