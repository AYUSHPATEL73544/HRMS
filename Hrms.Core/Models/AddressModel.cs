using Hrms.Core.Utilities;

namespace Hrms.Core.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        public int IndentificationId { get; set; }
        public Constants.AddressType AddressType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
    }
}
