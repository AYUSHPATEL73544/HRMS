using Hrms.Core.Abstractions;

namespace Hrms.Core.Entities
{
    public class EntityWithUserTracking<T> : IEntity<T>, IEntityWithUserTracking
    {
        public T Id { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
    }
}
