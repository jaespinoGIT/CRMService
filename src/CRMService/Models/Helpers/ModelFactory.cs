using CRMService.Core.Domain.Entities;

using Microsoft.AspNet.Identity.EntityFramework;

using System.Web.Http.Routing;

namespace CRMService.Models
{
    public class ModelFactory
    {

        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(System.Net.Http.HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserModel Create(User appUser)
        {
            return new UserModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),               
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };

        }

        public RoleModel Create(IdentityRole appRole)
        {
            return new RoleModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };

        }
    }  
}
