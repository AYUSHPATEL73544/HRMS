namespace Hrms.Core.Entities
{
    public class Country : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }

        public virtual ICollection<State> States { get; set; }

    }
}
