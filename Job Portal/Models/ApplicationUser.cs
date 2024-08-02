using Microsoft.AspNetCore.Identity;

namespace Job_Portal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; } = string.Empty;



    }
}
