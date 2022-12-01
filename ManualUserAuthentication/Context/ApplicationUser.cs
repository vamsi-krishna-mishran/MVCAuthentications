using Microsoft.AspNetCore.Identity;

namespace ManualUserAuthentication.Context
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public  override string Email { get; set; }
        public string Password { get; set; }
       
        public string ConfirmPassword { get; set; }
    }
}
