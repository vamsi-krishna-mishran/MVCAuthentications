using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MySqlCon.Context
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private const string AuthorizationHeaderName = "Authorizatioun";
        private const string BasicSchemeName = "Basic";
       // private readonly IBasicAuthenticationService _authenticationService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            /*IBasicAuthenticationService authenticationService*/)
            : base(options, logger, encoder, clock)
        {
           // _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();
            if (Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                //Authorization header not in request
                // return AuthenticateResult.NoResult();

               
                return AuthenticateResult.Fail("Invalid Basic authentication header");
            }

            if (AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderName], out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            //if (BasicSchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            //{
            //    //Not Basic authentication header
            //    return AuthenticateResult.NoResult();
            //}

            //byte[] headerValueBytes = Convert.FromBase64String(headerValue.Parameter);
            //string userAndPassword = Encoding.UTF8.GetString(headerValueBytes);
          //  string[] parts = userAndPassword.Split(':');
            if (false)//parts.Length == 2)
            {
                return AuthenticateResult.Fail("Invalid Basic authentication header");
            }
          ////  string user = parts[0];
           // string password = parts[1];

            bool isValidUser = true; //await IsValidUserAsync(user, password);

            if (!isValidUser)
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }
            var claims = new[] { new Claim(ClaimTypes.Name, "vamsi"),new Claim(ClaimTypes.Role,"user") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
    }
}
