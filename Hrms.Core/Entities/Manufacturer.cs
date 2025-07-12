
namespace Hrms.Core.Entities
{
   public class Manufacturer : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int AssetTypeId { get; set; }
        public virtual ICollection<Variant> Variants { get; set; }
    }
}
