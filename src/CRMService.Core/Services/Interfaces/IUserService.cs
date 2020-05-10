using CRMService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddUser(User user, bool userIsAdmin = false);
        Task<bool> DeleteUser(int userId);

        Task<User> GetUserAsync(int userId, bool includeUserRoles = false);

        Task<List<User>> GetAllUsersAsync(bool includeUserRoles = false);

        Task<User> UpdateUser(int userId, User user);
    }
}
