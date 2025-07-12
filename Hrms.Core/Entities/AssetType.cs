
namespace Hrms.Core.Entities
{
    public class AssetType : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public virtual ICollection<Manufacturer> Manufacturers { get; set; }

    }
}
