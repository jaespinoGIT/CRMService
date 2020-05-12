using AutoMapper;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services.Interfaces;
using CRMService.Models;
using Marvin.JsonPatch;
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
    public class UsersController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeUserRoles = false)
        {

            var result = await _userService.GetAllUsersAsync(includeUserRoles);

            if (result == null)
                return NotFound();
            // Mapping 
            var mappedResult = _mapper.Map<IEnumerable<UserModel>>(result);

            return Ok(mappedResult);

        }


        [Route("{userId}", Name = "GetUser")]
        public async Task<IHttpActionResult> Get(int userId, bool includeUserRoles = true)
        {

            var result = await _userService.GetUserAsync(userId, includeUserRoles);

            if (result == null)
                return NotFound();

            var mappedResult = _mapper.Map<UserModel>(result);

            return Ok(mappedResult);

        }

        [Route()]
        public async Task<IHttpActionResult> Post(UserModel model, bool userIsAdmin = false)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var user = await _userService.AddUser(_mapper.Map<User>(model), userIsAdmin);
                if (user != null)
                {
                    var newModel = _mapper.Map<UserModel>(user);

                    return CreatedAtRoute("GetUser", new { userId = newModel.UserId }, newModel);
                }
            }

            return BadRequest();
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

        [Route("{userId}")]
        public async Task<IHttpActionResult> Delete(int userId)
        {

            if (await _userService.DeleteUser(userId))
                return Ok();
            else
                return BadRequest();

        }
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
