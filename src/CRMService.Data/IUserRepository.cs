using CRMService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CRMService.Data
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
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(int userId);


        // UserRole
        void AddUserRole(UserRole userRole);
        void DeleteUserRole(UserRole userRole);
        Task<Role> GetRoleByUserIdAsync(int userId, int roleId);
        Task<Role[]> GetRolesByUserIdAsync(int userId);
    }
}