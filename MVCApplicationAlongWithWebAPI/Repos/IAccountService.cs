using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MVCApplicationAlongWithWebAPI.Models;
using MVCApplicationAlongWithWebAPI.Repository;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MVCApplicationAlongWithWebAPI.Repos
{
    public class ResetPwd
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public interface IAccountService
    {
        
        public Task<string> LogInAsync(SignInModel model);
        public Task<IdentityResult> SignUpAsync(SignUpModel model);  
        public Task<int> ForgetPasswordAsync(string email);
        public Task<int> ResetPasswordAsync(ResetPwd model);
    }
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;//used to get user and add user ..
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
       
        //private readonly IEmailService _emailService;
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,IEmailService emailService)
        {
            _userManager = userManager;//instantiating the user 
            _signInManager = signInManager;//signIn manager
            _config = configuration;//configuration object
            _emailService = emailService;  //email object 
        }
        public async Task<string> LogInAsync(SignInModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                //parameter1. email address, 2.password field, 3. auto login, 4. lock account after some attempts.
                if (!result.Succeeded)
                    return null;
                //    var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name,model.Email),
                //    new Claim("password",model.Password),
                //    new Claim(ClaimTypes.Role, "Administrator"),
                //    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //};
                //    var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Secret"]));
                //    var token = new JwtSecurityToken(
                //        issuer: _config["JWT:ValidIssuer"],
                //        audience: _config["JWT:ValidAudience"],
                //        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JWT:Expire"])),
                //        claims: authClaims,
                //        signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
                //        ); ;
                //    return new JwtSecurityTokenHandler().WriteToken(token);//sending a token to authorize to sources.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, "User"),
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(10),
                };
               
                return "success";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }
        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                ConfirmPassword = "",
                Password = ""

            };
            
            return await _userManager.CreateAsync(user, model.Password);
        }
        public async Task<int> ForgetPasswordAsync(string email)
        {
            email = email.ToUpper();//converts to upper case
            var user = await _userManager.FindByNameAsync(email);//if email found then that object will be return
            if (user == null)
            {
                return 0;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //above returns a token string to reset password..
            var encodedToken = Encoding.UTF8.GetBytes(token);//converting to byte array
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);// encoding so / will be not present
            string url = $"{_config["AppUrl"]}/Account/ResetPassword?email={email.ToLower()}&token={validToken}";
            //url to send the email, and token to reset password
            int status = _emailService.SendMail(email.ToLower(), "reset password", "<h1>follow instructions to reset password.</h1>" + $"to reset ur password <a href='{url}'>click here</a></p>");
            //sending mail using mailservice to the respective mail account..
            if (status == 0)
                return -1;//if fails
            return 1;//if mail send successfully..
        }
        public async Task<int> ResetPasswordAsync(ResetPwd model)
        //model with mail,token,password,confirmpassword fields.
        {
            var user = await _userManager.FindByNameAsync(model.Email.ToUpper());
            //checking for user existance.
            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);

            var Token = Encoding.UTF8.GetString(decodedToken);
            //decoding the encoded token earlier
            if (user == null)
            {
                return -1;//not existed..
            }
            var result = await _userManager.ResetPasswordAsync(user, Token, model.Password);
            //it validates user with token  and replaces the password..
            if (result.Succeeded)//if password get updated..
                return 1;//success
            return 0;//failed .

        }
    }
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
