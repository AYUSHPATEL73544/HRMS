using Hrms.Core.Utilities;


namespace Hrms.Core.Models.Assest
{
    public class AssetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsInWarranty { get; set; }
        public int WarrantyPeriod { get; set; }
        public string SerialNumber { get; set; }
        public string Variant { get; set; }
        public string Manufacturer { get; set; }
        public string VendorName { get; set; }
        public int VariantId { get; set; }
        public int AssetTypeId { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string VariantName { get; set; }
        public DateTime LastAssignDate { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
