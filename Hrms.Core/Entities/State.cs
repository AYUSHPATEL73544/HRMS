using Hrms.Core.Entities;

namespace Hrms.Core.Entities
{
    public class State : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
