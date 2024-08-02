using System.ComponentModel.DataAnnotations;

namespace Job_Portal.ViewModels
{

    public class ApplicationUserViewModel
    {

        public required string UserName { get; set;}
        public required string Password { get; set;}
        public required string Email { get; set;}
        public required string PhoneNumber { get; set;}
        public required string Address { get; set;}
        public required string Role { get; set;}
    }
}
