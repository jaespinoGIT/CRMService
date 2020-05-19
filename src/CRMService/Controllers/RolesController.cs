using CRMService.Models.Binding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Web.Http;
using System;
using System.Threading.Tasks;
using System.Web.Http;


namespace CRMService.Controllers
{
    /// <summary>
    /// Roles operations controller, only admins
    /// </summary>   
    [ApiVersion("1.0")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        /// GET: api/roles/{roleId}
        /// <summary>
        /// Gets an role by id.
        /// </summary>
        /// <remarks>
        /// Gets all data of an role.
        /// </remarks>  
        /// <param name="roleId">Id (GUID) of the role.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response role.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Authorize(Roles = "Admin")]
        [Route("{roleId:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string roleId)
        {
            var role = await this.AppRoleManager.FindByIdAsync(roleId);

            if (role != null)           
                return Ok(TheModelFactory.Create(role));           

            return NotFound();
        }
        /// GET: api/roles
        /// <summary>
        /// Gets all registered roles.
        /// </summary>
        /// <remarks>
        /// Gets all registered roles.
        /// </remarks>       
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response roles list.</response>  
        [Authorize(Roles = "Admin")]
        [Route()]       
        public async Task<IHttpActionResult> Get()
        {
            var roles = this.AppRoleManager.Roles;

            return Ok(roles);
        }
        /// POST: api/roles
        /// <summary>
        /// Add a new role
        /// </summary>
        /// <remarks>
        /// Add a new role to the db
        /// </remarks>
        /// <param name="model"> Role to be created.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>                            
        /// <response code="201">Created. Role created.</response>        
        /// <response code="400">BadRequest. Wrong object format.  .</response>
        /// <response code="409">Conflict. There is an role already in the db.</response>
        [Authorize(Roles = "SuperAdmin")]
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
        /// DELETE: api/roles/{roleId}
        /// <summary>
        /// Delete role by id
        /// </summary>
        /// <remarks>
        /// Delete role by id
        /// </remarks> 
        /// <param name="roleId">Id (GUID) of the role.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Role deleted.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Authorize(Roles = "SuperAdmin")]
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
        /// GET: api/roles/ManageUsersInRole
        /// <summary>
        /// Adds users to roles
        /// </summary>
        /// <remarks>
        /// Adds and delete users of roles
        /// </remarks>  
        /// <param name="model">Id (GUID) of the role.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. </response>  
        /// <response code="404">NotFound. User not found.</response>
        [Authorize(Roles = "Admin")]
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
