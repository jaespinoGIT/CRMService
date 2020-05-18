using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CRMService.Helpers.CustomExtensions
{
    public class RolesFromClaims
    {
        /// <summary>
        /// Adds new claims based on roles and another claims
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static IEnumerable<Claim> CreateRolesBasedOnClaims(ClaimsIdentity identity)
        {
            List<Claim> claims = new List<Claim>();

            if (identity.HasClaim(c => c.Type == "FirstUser" && c.Value == "1") &&
                identity.HasClaim(ClaimTypes.Role, "Admin"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Creator"));
            }

            return claims;
        }

    }
}
