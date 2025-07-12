using Hrms.Core.Utilities;

namespace Hrms.Core.Entities
{
    public class Address : Entity<int>
    {
        public int IdentificationId { get; set; }
        public Constants.AddressType AddressType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Landmark { get; set; }
        public int CityId { get; set; }     
        public string PinCode { get; set; }
    }
}
