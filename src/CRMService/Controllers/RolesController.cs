using CRMService.Models.Binding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Web.Http;
using System;
using System.Threading.Tasks;
using System.Web.Http;


namespace CRMService.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {

        [Route("{roleId:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string roleId)
        {
            var role = await this.AppRoleManager.FindByIdAsync(roleId);

            if (role != null)           
                return Ok(TheModelFactory.Create(role));           

            return NotFound();
        }

        [Route()]       
        public async Task<IHttpActionResult> Get()
        {
            var roles = this.AppRoleManager.Roles;

            return Ok(roles);
        }

        [Route()]
        public async Task<IHttpActionResult> Post(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)          
                return BadRequest(ModelState);            

            var role = new IdentityRole { Name = model.Name };

            var result = await this.AppRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }           

            return CreatedAtRoute("GetRoleById", new { roleId = role.Id }, TheModelFactory.Create(role));  
        }

        [Route("{roleId:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string roleId)
        {

            var role = await this.AppRoleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                IdentityResult result = await this.AppRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await this.AppRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
            {
                var appUser = await this.AppUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!this.AppUserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await this.AppUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var appUser = await this.AppUserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await this.AppUserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
