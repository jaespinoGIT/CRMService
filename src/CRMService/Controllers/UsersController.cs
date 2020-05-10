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
            try
            {
                var result = await _userService.GetAllUsersAsync(includeUserRoles);

                if (result == null)
                    return NotFound();
                // Mapping 
                var mappedResult = _mapper.Map<IEnumerable<UserModel>>(result);

                return Ok(mappedResult);
            }
            catch
            {
                return InternalServerError();
            }
        }

      
        [Route("{userId}", Name = "GetUser")]
        public async Task<IHttpActionResult> Get(int userId, bool includeUserRoles = true)
        {
            try
            {
                var result = await _userService.GetUserAsync(userId, includeUserRoles);

                if (result == null)
                    return NotFound();

                var mappedResult = _mapper.Map<UserModel>(result);

                return Ok(mappedResult);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(UserModel model, bool userIsAdmin = false)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    var user = _mapper.Map<User>(model);

                    if (await _userService.AddUser(user, userIsAdmin))
                    {
                        var newModel = _mapper.Map<UserModel>(user);

                        return CreatedAtRoute("GetUser", new { userId = newModel.UserId }, newModel);
                    }
                }                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{userId}")]
        public async Task<IHttpActionResult> Put(int userId, UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                var user = _mapper.Map<User>(model);

                var userUpdated = await _userService.UpdateUser(userId, user);

                if (userUpdated == null)
                    return InternalServerError();
                else
                    return Ok(_mapper.Map<CustomerModel>(userUpdated));             
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{userId}")]
        public async Task<IHttpActionResult> Delete(int userId)
        {
            try
            {
                if (await _userService.DeleteUser(userId))
                    return Ok();
                else
                    return BadRequest();            
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("{userId}")]       
        public async Task<IHttpActionResult> Patch(int userId, [FromBody] JsonPatchDocument<UserModel> patchDoc)
        {
            try
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

                var userUpdated = await _userService.UpdateUser(userId, user);

                if (userUpdated == null)
                    return InternalServerError();
                else
                    return Ok(_mapper.Map<CustomerModel>(userUpdated));             
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            } 
        }

        [Route("{userId}")]
        [HttpPatch]
        public async Task<IHttpActionResult> ChangeAdminStatus(int userId, bool newAdminStatus = false)
        {
            try
            {            

                var user = await _userService.GetUserAsync(userId, false);
                if (user == null) return NotFound();

                var userModelToPatch = _mapper.Map<UserModel>(user);


                // Assign entity changes to original entity retrieved from database
                _mapper.Map(userModelToPatch, user);

                var userUpdated = await _userService.UpdateUser(userId, user);

                if (userUpdated == null)
                    return InternalServerError();
                else
                    return Ok(_mapper.Map<CustomerModel>(userUpdated));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
