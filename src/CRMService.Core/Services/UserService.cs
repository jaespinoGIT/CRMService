using CRMService.Core.Domain.Entities;
using CRMService.Core.Exceptions.Services;
using CRMService.Core.Repositories;
using CRMService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICustomExceptionService _customExceptionService;
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ICustomExceptionService customExceptionService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _customExceptionService = customExceptionService;
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

        public async Task<User> AddUser(User user, bool userIsAdmin = false)
        {
            if (await _userRepository.GetUserByLoginAsync(user.Login) != null)
            {
                _customExceptionService.ThrowInvalidOperationException("User with the same login exists");
                return null;
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

                    if (await _userRepository.SaveChangesAsync())
                        return user;
                    else
                        _customExceptionService.ThrowInvalidOperationException("Error adding user");
                }
            }
            return null;

        }

        public async Task<User> UpdateUser(User user)
        {   
            if (await _userRepository.SaveChangesAsync())            
                return user;            
            else
            {
                _customExceptionService.ThrowInvalidOperationException("Error updating user");
                return null;
            }
        }

        public async Task<User> ChangeAdminStatus(int userId, bool newAdminStatus)
        {
            var user = await _userRepository.GetUserAsync(userId, true);
            if (user == null)
            {
                _customExceptionService.ThrowItemNotFoundException("User doesnt exists");
                return null;
            }

            List<UserRole> userRolesList = user.UserRoles.ToList();
            if (userRolesList != null &&
                userRolesList.Exists(ur => ur.Role.RoleIsAdmin == newAdminStatus))
            {
                _customExceptionService.ThrowInvalidOperationException("User's actual admin status is the same");
                return null;
            }
           
            List<Role> rolesList = await _roleRepository.GetAllRolesAsync();
           
            if (rolesList != null)
            {
                Role role = rolesList.Find(r => r.RoleIsAdmin == newAdminStatus);                

                if (role != null)
                {
                    userRolesList[0].Role = role;
                    
                    user.UserRoles = new Collection<UserRole>(userRolesList);

                    if (await _userRepository.SaveChangesAsync())
                        return user;
                    else
                    {
                        _customExceptionService.ThrowInvalidOperationException("Error updating user");
                        return null;
                    }
                }
            }
            return null;

        }

    }
}
