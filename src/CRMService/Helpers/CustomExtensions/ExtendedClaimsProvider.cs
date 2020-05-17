using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using CRMService.Core.Domain.Entities;

namespace CRMService.Helpers.CustomExtensions
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            
            if (user.UserName == "SuperPowerUser")           
                claims.Add(CreateClaim("FirstUser", "1"));
           
            else          
                claims.Add(CreateClaim("FirstUser", "0"));
                      

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}
