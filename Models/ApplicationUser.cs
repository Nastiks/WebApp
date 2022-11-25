using Microsoft.AspNetCore.Identity;

namespace WebApplic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisterTimeStamp { get; set; }
        public DateTime LastLogin { get; set; }
        public bool Blocked { get; set; }
    }
}
