using Hrms.Core.Utilities;
using Microsoft.AspNetCore.Identity;

namespace Hrms.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? LastLoggedOn { get; set; }

        public virtual Employee Employee { get; set; } 
    }
}
