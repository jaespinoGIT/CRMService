using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> GetUserAsync(int userId, bool includeUserRoles = false)
        {
            var user = await _userRepository.GetUserAsync(userId, includeUserRoles);

            return user;
        }

        public async Task<List<User>> GetAllUsersAsync(bool includeUserRoles = false)
        {
            return await _userRepository.GetAllUsersAsync(includeUserRoles);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _userRepository.GetUserAsync(userId, true);
            if (user == null) return false;

            if (user.UserRoles != null)
            foreach (var item in user.UserRoles)
            {
                _userRepository.DeleteUserRole(item);
            }

            _userRepository.DeleteUser(user);

            return await _userRepository.SaveChangesAsync();

        }

        public async Task<bool> AddUser(User user, bool userIsAdmin = false)
        {
            if (await _userRepository.GetUserByLoginAsync(user.Login) != null)
            {
                return false;
                //ModelState.AddModelError("Name", "Name in use");   
            }
            //Get default user role
            List<Role> rolesList = await _roleRepository.GetAllRolesAsync();

            Role role = null;
            if (rolesList != null)
            {
                role = rolesList.Find(r => r.RoleIsAdmin == userIsAdmin);

                if (role != null)
                {
                    user.UserRoles = new UserRole[]
                    {
                          new UserRole()
                          {
                              User = user,
                              Role = role
                          }
                    };

                    _userRepository.AddUser(user);

                    return await _userRepository.SaveChangesAsync();
                }
            }
            return false;

        }

        public async Task<User> UpdateUser(int userId, User userUpdate)
        {

            var user = await _userRepository.GetUserAsync(userId);
            if (user == null) return null;

            userUpdate.UserId = userId;

            user = userUpdate;

            if (await _userRepository.SaveChangesAsync())
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
