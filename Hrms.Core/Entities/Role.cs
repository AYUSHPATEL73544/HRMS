using Microsoft.AspNetCore.Identity;

namespace Hrms.Core.Entities
{
    public class Role<T> : IdentityRole<int>
    {
        public string DisplayName { get; set; }
    }
}
