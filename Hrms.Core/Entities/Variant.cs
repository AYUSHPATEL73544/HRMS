
namespace Hrms.Core.Entities
{
    public class Variant : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
    }
}
