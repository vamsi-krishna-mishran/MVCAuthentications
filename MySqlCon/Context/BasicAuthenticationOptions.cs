using Microsoft.AspNetCore.Authentication;

namespace MySqlCon.Context
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }

}
