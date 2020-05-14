using AutoMapper;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services.Interfaces;
using CRMService.Models;
using CRMService.Models.Binding;
using Marvin.JsonPatch;
using Microsoft.AspNet.Identity;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRMService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get()
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));

        }

        [Route("{userId}", Name = "GetUser")]
        public async Task<IHttpActionResult> Get(string userId)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByIdAsync(userId);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }


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

            return CreatedAtRoute("GetUser", new { userId = newModel.Id }, newModel);
        }

        [Route("{userId}")]
        public async Task<IHttpActionResult> Put(int userId, UserModel model)
        {
            var user = await _userService.GetUserAsync(userId, true);

            _mapper.Map(model, user);

            var userUpdated = await _userService.UpdateUser(user);

            if (userUpdated == null)
                return InternalServerError();
            else
                return Ok(_mapper.Map<UserModel>(userUpdated));

        }

        //[Route("{userId}")]
        //public async Task<IHttpActionResult> Delete(int userId)
        //{

        //    if (await _userService.DeleteUser(userId))
        //        return Ok();
        //    else
        //        return BadRequest();

        //}
        [Route("{userId}")]
        public async Task<IHttpActionResult> Patch(int userId, [FromBody] JsonPatchDocument<UserModel> patchDoc)
        {

            // If the received data is null
            if (patchDoc == null)
                return BadRequest();

            var user = await _userService.GetUserAsync(userId, false);
            if (user == null) return NotFound();

            var userModelToPatch = _mapper.Map<UserModel>(user);

            patchDoc.ApplyTo(userModelToPatch);

            // Assign entity changes to original entity retrieved from database
            _mapper.Map(userModelToPatch, user);

            var userUpdated = await _userService.UpdateUser(user);

            if (userUpdated == null)
                return InternalServerError();
            else
                return Ok(_mapper.Map<UserModel>(userUpdated));

        }

        [HttpPut]
        [Route("{userId}/changeadminstatus")]
        public async Task<IHttpActionResult> ChangeAdminStatus(int userId, bool newAdminStatus = true)
        {

            var userUpdated = await _userService.ChangeAdminStatus(userId, newAdminStatus);

            if (userUpdated == null)
                return InternalServerError();
            else
                return Ok(_mapper.Map<UserModel>(userUpdated));

        }
    }
}
