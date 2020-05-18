using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRMService.Controllers
{
    /// <summary>
    /// Claims related ontroller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/claims")]
    [ApiVersion("1.0")]
    public class ClaimsController : BaseApiController
    {       /// GET: api/claims
            /// <summary>
            /// Gets all claims of current user.
            /// </summary>
            /// <remarks>
            /// Gets all claims of current user.
            /// </remarks>       
            /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
            /// <response code="200">OK. Response user claims.</response>          
        public IHttpActionResult GetClaims()
        {
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            return Ok(claims);
        }

    }
}
