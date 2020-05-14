using CRMService.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMService.Core.Repositories
{
    public interface IUserRepository
    {
        //IQueryable<User> GetQueryable();

        //void CreateUser(User user);

        List<User> GetAll();

        Task<bool> SaveChangesAsync();

        // Users
        void AddUser(User user);
        void DeleteUser(User user);
        Task<List<User>> GetAllUsersAsync(bool includeUserRoles = false);
        Task<User> GetUserAsync(int userId, bool includeUserRoles = true);
        Task<User> GetUserByLoginAsync(string login, bool includeUserRoles = false);

        //User roles

        void AddUserRole(UserRole userRole);

        void DeleteUserRole(UserRole userRole);
       
    }
}