using System.Text.Json.Serialization;

namespace Hrms.Core.Models.Account
{
    public class GraphUserModel
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }
        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }
        [JsonPropertyName("mail")]
        public string Mail { get; set; }
        [JsonPropertyName("mobilePhone")]
        public string MobilePhone { get; set; }
        [JsonPropertyName("officeLocation")]
        public string OfficeLocation { get; set; }
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }
    }
}
