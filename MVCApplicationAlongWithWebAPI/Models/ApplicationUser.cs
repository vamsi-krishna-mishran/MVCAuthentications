using Microsoft.AspNetCore.Identity;

namespace MVCApplicationAlongWithWebAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
