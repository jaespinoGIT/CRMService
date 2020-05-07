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
    }
}