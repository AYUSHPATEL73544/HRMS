using Hrms.Core.Abstractions;
using Hrms.Core.Utilities;

namespace Hrms.Core.Entities
{
    public class EntityWithStatusTracking<T> : IEntity<T>, IEntityWithStatusTracking
    {
        public T Id { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
