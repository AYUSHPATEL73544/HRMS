using Hrms.Core.Abstractions;

namespace Hrms.Core.Entities
{
    public class EntityWithTimeTracking<T> : IEntity<T>, IEntityWithTimeTracking
    {
        public T Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
