using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCApplicationAlongWithWebAPI.Models;
using MVCApplicationAlongWithWebAPI.Repos;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MVCApplicationAlongWithWebAPI.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResetPassword(string Email,string token)
        {
            ViewBag.Email = Email;
            ViewBag.Token = token;
            return View();
        }
        [HttpPost]
        public async Task<string> ResetPassword([FromForm] ResetPwd obj)
        {

            int status =await _accountService.ResetPasswordAsync(obj);
            if (status == 1)
            {
                return JsonConvert.SerializeObject(new { IsSuccess = true, status = "resetted successfully" });
            }
            return JsonConvert.SerializeObject(new { IsSuccess = false, status = "resetting is unsuccessful" });

        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> ForgetPassword(string email)
        {
            if (email == null)
            {
                return JsonConvert.SerializeObject(new { success = -1, message = "email field is empty" });
            }
            int result = await _accountService.ForgetPasswordAsync(email);
            if (result == 1)
                return JsonConvert.SerializeObject(new { success = 1, message = "done well" });
            else if (result == 0) return JsonConvert.SerializeObject(new { success = 0, message = "user not found" });
            else if (result == -1) return JsonConvert.SerializeObject(new { success = -1, message = "invalid credentials" });
            return JsonConvert.SerializeObject(new { success = 1, message = "other error" });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("_UserName");
            return View("LogIn");
        }
        public IActionResult LogIn(int IsSuccess=0)
        {
            ViewBag.IsSuccess = IsSuccess;
            return View();
        }
        [HttpPost]
        public async Task<String> LogIn([FromBody] SignInModel model)
        {

            if (!ModelState.IsValid)
            {
                return JsonConvert.SerializeObject(new { IsSuccess = 0, token = "" });
            }
            string result = await _accountService.LogInAsync(model);
            if (result==null)
            {
                //return RedirectToAction("LogIn", new { IsSuccess = -1 });
                return JsonConvert.SerializeObject(new { IsSuccess = 0,token="" });
            }
            else if(result == "success")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,model.Email),
                    new Claim(ClaimTypes.Role,"user")
                };
                var identity=new ClaimsIdentity(claims);
                var principle=new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principle, props);
                //HttpContext.Session.SetString("_UserName", model.Email);
                return JsonConvert.SerializeObject(new { IsSuccess = 1, token = result });
            }
            return JsonConvert.SerializeObject(new { IsSuccess = 0, token = "" });
            //return JsonConvert.SerializeObject(new { IsSuccess = 1, token = result });

        }
        public ViewResult SignUp(int IsOkay=0,string IsStatus="")
        {
            //ViewBag.Status = status;
            ViewBag.IsStatus = IsStatus;
            ViewBag.IsOkay = IsOkay;
            return View();
        }
        [HttpPost]
        public async Task<string> SignUp([FromBody]SignUpModel model) {
            if (!ModelState.IsValid)
            {
                
                return JsonConvert.SerializeObject(new { Issuccess = 0, message = "Enter all the fields." });
            }
            var result =await _accountService.SignUpAsync(model);
            if (result.Succeeded)
            {
                
                return JsonConvert.SerializeObject(new { Issuccess = 1, message = "Registration is successful" });
            }
            else
            {
                var errors = result.Errors;
                string singleString = string.Join<IdentityError>(",", errors.ToArray());
                return JsonConvert.SerializeObject(new { Issuccess = -1, message = singleString });
            }
        }
    }
}
