
namespace Hrms.Core.Entities
{
    public class AssetAllocation : EntityBase<int>
    {
        public int EmployeeId { get; set; }
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
