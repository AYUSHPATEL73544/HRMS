using Hrms.Core.Abstractions;
using Hrms.Core.Utilities;

namespace Hrms.Core.Entities
{
    public class EntityBase<T> : IEntity<T>, IEntityWithStatusTracking, IEntityWithTimeTracking, IEntityWithUserTracking
    {
        public T Id { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
