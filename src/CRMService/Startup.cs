using System;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;
using CRMService.Helpers.Auth;
using CRMService.Helpers.Loggers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(CRMService.Startup))]

namespace CRMService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();


            //var myTokenProvider = new TokenBaseAuthenticationProvider();
            //OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    Provider = myTokenProvider
            //};
            //app.UseOAuthAuthorizationServer(options);
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
