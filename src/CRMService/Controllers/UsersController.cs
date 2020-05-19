
using CRMService.Core.Domain.Entities;
using CRMService.Helpers.Auth;
using CRMService.Helpers.CustomExtensions;
using CRMService.Models.Binding;

using Microsoft.AspNet.Identity;
using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRMService.Controllers
{
    /// <summary>
    /// User operations controller, only admins
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    [RoutePrefix("api/users")]    
    public class UsersController : BaseApiController
    {
        /// GET: api/users
        /// <summary>
        /// Gets all registered users.
        /// </summary>
        /// <remarks>
        /// Gets all registered users.
        /// </remarks>       
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response users list.</response>  
        [Route()]
        public async Task<IHttpActionResult> Get()
        {   
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        /// GET: api/users/{userId}
        /// <summary>
        /// Gets an user by id.
        /// </summary>
        /// <remarks>
        /// Gets all data of an user.
        /// </remarks>  
        /// <param name="userId">Id (GUID) of the user.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response user.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Route("{userId}", Name = "GetUserById")]
        public async Task<IHttpActionResult> Get(string userId)
        {
          
            var user = await this.AppUserManager.FindByIdAsync(userId);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();
        }
        /// GET: api/users/user/{userName}
        /// <summary>
        /// Gets an user by username.
        /// </summary>
        /// <remarks>
        /// Gets all data of an user.
        /// </remarks> 
        /// <param name="username">Id (GUID) of the user.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response user.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
           
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        /// POST: api/users
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <remarks>
        /// Add a new user to the db
        /// </remarks>
        /// <param name="createUserModel"> User to be created.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>                            
        /// <response code="201">Created. User created.</response>        
        /// <response code="400">BadRequest. Wrong object format.  .</response>
        /// <response code="409">Conflict. There is an user already in the db.</response>
        [Route()]
        public async Task<IHttpActionResult> Post(CreateUserBindingModel createUserModel)
        {
            Validate(createUserModel);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }
            var newModel = TheModelFactory.Create(user);

            return CreatedAtRoute("GetUserById", new { userId = newModel.Id }, newModel);
        }

        /// PUT: api/users/ChangePassword
        /// <summary>
        /// Change the password of the current user
        /// </summary>
        /// <remarks>
        /// Change the password of the current user
        /// </remarks>     
        /// <param name="model"> Old and new password.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Password changed</response>         
        /// <response code="400">BadRequest. Wrong object format.</response>
        [Route("user/ChangePassword")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// DELETE: api/users/{userId}
        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <remarks>
        /// Delete user by id
        /// </remarks> 
        /// <param name="userId">Id (GUID) of the user.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. User deleted.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Delete(string userId)
        {      
            var appUser = await this.AppUserManager.FindByIdAsync(userId);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();
        }

        /// PUT:  api/users/{userId}/roles
        /// <summary>
        /// Assign new role to the user.
        /// </summary>
        /// <remarks>
        /// Assign new role to the user.
        /// </remarks> 
        /// <param name="id">Id (GUID) of the user.</param>
        /// <param name="rolesToAssign">List (string) of new roles.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Roles assigned.</response>  
        /// <response code="404">NotFound. User not found.</response>
        /// <response code="400">BadRequest. Roles doesnt exist.</response>
        [Route("{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();

        }
        /// PUT:  api/users/{userId}/assignclaims
        /// <summary>
        /// Assign new claims to the user.
        /// </summary>
        /// <remarks>
        /// Assign new claims and remove old claims to the user.
        /// </remarks> 
        /// <param name="id">Id (GUID) of the user.</param>
        /// <param name="claimsToAssign">List (ClaimBindingModel) of new claims.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Claims assigned.</response>  
        /// <response code="404">NotFound. User not found.</response>
        /// <response code="400">BadRequest. Wrong object format.</response>
        [Route("{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }
        /// PUT:  api/users/{userId}/removeclaims
        /// <summary>
        /// Remove old claims to the user.
        /// </summary>
        /// <remarks>
        /// Remove old claims to the user.
        /// </remarks> 
        /// <param name="id">Id (GUID) of the user.</param>
        /// <param name="claimsToRemove">List (ClaimBindingModel) of new claims.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Claims removed.</response>  
        /// <response code="404">NotFound. User not found.</response>
        /// <response code="400">BadRequest. Wrong object format.</response>
        [Route("{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }




    }
}
