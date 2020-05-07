using AutoMapper;
using CRMService.Data;
using CRMService.Models;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repository, IMapper mapper)
        {
            _userRepository = repository;
            _mapper = mapper;
        }

        [Route()]       
        public async Task<IHttpActionResult> Get(bool includeUserRoles = false)
        {
            try
            {
                var result = await _userRepository.GetAllUsersAsync(includeUserRoles);

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
                var result = await _userRepository.GetUserAsync(userId, includeUserRoles);

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

        [Route("{userId}")]
        public async Task<IHttpActionResult> Put(int userId, UserModel model)
        {
            try
            {
                var user = await _userRepository.GetUserAsync(userId);
                if (user == null) return NotFound();

                _mapper.Map(model, user);

                if (await _userRepository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<UserModel>(user));
                }
                else
                {
                    return InternalServerError();
                }
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
                var user = await _userRepository.GetUserAsync(userId);
                if (user == null) return NotFound();

                _userRepository.DeleteUser(user);

                if (await _userRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
