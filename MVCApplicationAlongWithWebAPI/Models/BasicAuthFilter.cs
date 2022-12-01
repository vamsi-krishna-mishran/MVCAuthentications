using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Http.Extensions;

namespace MVCApplicationAlongWithWebAPI.Models
{
    public class BasicAuthFilter:IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string MethodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var url = context.HttpContext.Request.GetEncodedUrl();
                Console.WriteLine(url);
                string authHeader = context.HttpContext.Request.Headers["token"];//accessing token header of request.
                if (authHeader != null)
                {
                    if (true)//BasicAuthFilter.ValidateToken(authHeader))
                        return;
                    else//raise Unauthorized error..
                        UnauthorizedResult(context);
                }
                else
                    UnauthorizedResult(context);
            }
            catch (FormatException ex)//if no token present ,,then raise Unauthorized error..
            {
               // JsonController.JsonController_Err(ex, MethodName);
                UnauthorizedResult(context);
            }
        }
        
        private void UnauthorizedResult(AuthorizationFilterContext context)
        {
            // context.HttpContext.Response.Headers
            context.Result = new UnauthorizedResult();
            //context.HttpContext.Response.Headers
        }
        //public static bool ValidateToken(string token)
        //{
        //    bool Status = false;
        //    try
        //    {
        //        if (token != "")
        //        {
        //            string Err = "";
        //            DateTime currentTime = DateTime.Now;
        //            token = token.Replace('*', '/');
        //            token = token.Replace('$', '+');
        //            string Data = "";// AESEncrytDecry.DecryptStringAES(token);
        //            var Data_Split = Data.Split(";");
        //            string UserId = Data_Split[0];
        //            if (true)//MiddleTyre_Mysql.CheckUser(UserId, ref Err))
        //            {
        //                string DateTime_Input = Data_Split[1];
        //                var DateTime_Input_Split = DateTime_Input.Split(":");
        //                DateTime dt = DateTime.Now;
        //                DateTime dt_Compare = new DateTime(Convert.ToInt32(DateTime_Input_Split[2]), Convert.ToInt32(DateTime_Input_Split[1]), Convert.ToInt32(DateTime_Input_Split[0]),
        //                                                Convert.ToInt32(DateTime_Input_Split[3]), Convert.ToInt32(DateTime_Input_Split[4]), Convert.ToInt32(DateTime_Input_Split[5]));
        //                var Diff = (dt - dt_Compare);
        //                var Minutes = (dt - dt_Compare).TotalMinutes;
        //                var Days = (dt - dt_Compare).Days;
        //                if (Days == 0)
        //                {
        //                    if (Minutes > 0 && Minutes <= Common.SessionTime)
        //                    {
        //                        Status = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.ToString());
        //        Status = false;
        //    }
        //    return Status;
        //}
    }
}
