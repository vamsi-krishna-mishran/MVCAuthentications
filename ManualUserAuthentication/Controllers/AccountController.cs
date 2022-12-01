using ManualUserAuthentication.Context;
using ManualUserAuthentication.Models;
using ManualUserAuthentication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ManualUserAuthentication.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService,BooksContext context)//,Books book)
        {
            _accountService = accountService;
            
           // DateTime dt = new DateTime(2020, 12, 01);
           // DateTime dt2 = new DateTime(2021, 12, 05);
           // Console.WriteLine("in time");
           // Console.WriteLine(dt.Subtract(dt2).TotalMinutes);
        }
        [HttpPost]
        public  IActionResult LogIn(SignInModel model)
        {
            int response = _accountService.LogInAsync(model.Email,model.Password);

            if (response == -1)
                return NotFound("User is not present.");
            else if(response == 0)
                return Unauthorized("Invalid Password Credentials..");
            else
            {
                //getting current time to generate token
                string startTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                //adding username to start time
                string token = model.Email + "^" + startTime;

                //converting to bytes
                var TokenBytes = Encoding.UTF8.GetBytes(token);//converting to byte array

                //encoding
                var validToken = WebEncoders.Base64UrlEncode(TokenBytes);

                //creating object to return the token
                var obj = new {token=validToken};

               // double MinutesDifference=DifferenceInMinutes(DateTime.Now, validToken);

              //  Console.WriteLine(MinutesDifference);
                return Ok(obj);
            }

        }
        public static double DifferenceInMinutes(DateTime currentTime,string token)
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            //getting decrypted token..which is username+tokenGenerationTime
            string ResultToken = Encoding.UTF8.GetString(decodedTokenBytes);
            string decodedTokenTime = ResultToken.Split("^")[1];
            DateTime TokenTime = DateTime.ParseExact(decodedTokenTime, "MM-dd-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            double minuteDifference=currentTime.Subtract(TokenTime).TotalMinutes;

            return minuteDifference;
        }
    }
}
