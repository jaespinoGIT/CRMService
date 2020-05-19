using CRMService.Core.Domain.Entities;
using CRMService.Models.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Web.Http.Routing;

namespace CRMService.Models
{
    public class ModelFactory : IModelFactory
    {

        private UrlHelper _urlHelper;
        private ApplicationUserManager _appUserManager;

        public ModelFactory(System.Net.Http.HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _urlHelper = new UrlHelper(request);
            _appUserManager = appUserManager;
        }
        /// <summary>
        /// Maps entity user to UserModel
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public UserModel Create(User appUser)
        {
            return new UserModel
            {
                Url = _urlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),               
                Roles = _appUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _appUserManager.GetClaimsAsync(appUser.Id).Result
            };

        }

        public RoleModel Create(IdentityRole appRole)
        {
            return new RoleModel
            {
                Url = _urlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };

        }
    }  
}
