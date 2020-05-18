
using CRMService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

using System.Net.Http;
using System.Web.Http;
using CRMService.Models.Helpers;

namespace CRMService.Controllers
{
    public class BaseApiController : ApiController
    {

        private IModelFactory _modelFactory;
        private ApplicationUserManager _appUserManager = null;
        private ApplicationRoleManager _appRoleManager = null;

        public ApplicationUserManager AppUserManager
        {
            get
            {
                return _appUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set //public for tests only

            {
                _appUserManager = value;
            }
        }

        public ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _appRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            set //public for tests only

            {
                _appRoleManager = value;
            }
        }

        internal BaseApiController()
        {

        }    

        public IModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
            set //public for tests only
            {
                _modelFactory = value;
            }
        }

        internal IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
