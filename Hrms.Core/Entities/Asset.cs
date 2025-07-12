
namespace Hrms.Core.Entities

{
    public class Asset : EntityWithStatusTracking<int>
    {
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsInWarranty { get; set; }
        public int WarrantyPeriod { get; set; }
        public string SerialNumber { get; set; }
        public int VariantId { get; set; }
        public int ManufacturerId { get; set; }
        public int AssetTypeId { get; set; }
        public string VendorName { get; set; }
        public  virtual Variant Variant { get; set; }  
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual AssetType AssetType { get; set; } 
        
    }
}
