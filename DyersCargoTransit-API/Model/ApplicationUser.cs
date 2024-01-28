using Microsoft.AspNetCore.Identity;

namespace DyersCargoTransit_API.Model
{
    public class ApplicationUser:IdentityUser
    {
        
        public virtual UserProfile UserProfile { get; set; }
    }
}
