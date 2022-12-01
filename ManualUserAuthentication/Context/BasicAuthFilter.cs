using ManualUserAuthentication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ManualUserAuthentication.Context
{
    public class BasicAuthFilter:IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["token"];//accessing token header of request.
                if (authHeader != null)
                {
                    // var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (AccountController.DifferenceInMinutes(DateTime.Now, authHeader) <= 1)//if dif.less than 1,then continue.
                        return;
                    else//if time difference is more than one minute ..raise Unauthorized error..
                        UnauthorizedResult(context);
                }
                else
                    UnauthorizedResult(context);
            }
            catch (FormatException)//if no token present ,,then raise Unauthorized error..
            {
                UnauthorizedResult(context);
            }
        }
        private void UnauthorizedResult(AuthorizationFilterContext context)
        {
            // context.HttpContext.Response.Headers
            context.Result = new UnauthorizedResult();
        }
    }
}
