using CRMService.Infrastructure.Data.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CRMService.Infrastructure.Data.EntityFramework.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetQueryable();

        void CreateUser(User user);

        List<User> GetAll();

        Task<bool> SaveChangesAsync();

        // Users
        void AddUser(User user);
        void DeleteUser(User user);
        Task<List<User>> GetAllUsersAsync(bool includeUserRoles = false);
        Task<User> GetUserAsync(int userId, bool includeUserRoles = true);
        Task<User> GetUserByLoginAsync(string login, bool includeUserRoles = true);

        // UserRole
        void AddUserRole(UserRole userRole);
        void DeleteUserRole(UserRole userRole);
        Task<Role> GetRoleByUserIdAsync(int userId, int roleId);
        Task<Role[]> GetRolesByUserIdAsync(int userId);
    }
}