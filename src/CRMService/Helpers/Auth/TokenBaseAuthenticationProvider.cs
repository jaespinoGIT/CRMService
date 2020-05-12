using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Helpers.Auth
{
    public class TokenBaseAuthenticationProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            //using (var cxt = new TestEntities2())
            //{
            //    var userdata = cxt.EF_UserLogin(context.UserName, context.Password).FirstOrDefault();



            //    if (userdata != null)
            //    {
            //        identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role as string, userdata.UserRole as string));
            //        identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name as string, userdata.UserName as string));
            //        context.Validated(identity);
            //    }
            //    else
            //    {
            //        context.SetError("invalid_grant", "Provided username and password is incorrect");
            //        context.Rejected();
            //    }
            //}
        }

    }
}
