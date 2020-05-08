using CRMService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace CRMService.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;


        private readonly DbSet<User> _dbSet;

        public UserRepository()
        {

        }
        public UserRepository(DbSet<User> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<User> GetQueryable()
        {
            return _dbSet;
        }

        public void CreateUser(User user)
        {
            _dbSet.Add(user);
        }

        public List<User> GetAll()
        {
            return _dbSet.AsQueryable().ToList();
        }

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void AddUserRole(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public void DeleteUserRole(UserRole userRole)
        {
            _context.UserRoles.Remove(userRole);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<List<User>> GetAllUsersAsync(bool includeUserRoles = false)
        {
            IQueryable<User> query = _context.Users;

            if (includeUserRoles)
            {
                query = query.Include(c => c.UserRoles.Select(r => r.Role));
                query = query.Include(c => c.UserRoles.Select(r => r.User));
            }

            // Order It
            //query = query.OrderByDescending(c => c.Name);

            return await query.ToListAsync();

        }

        public async Task<User> GetUserAsync(int userId, bool includeUserRoles = false)
        {
            IQueryable<User> query = _context.Users;
            //Add user roles
            if (includeUserRoles)
            {
                query = query.Include(c => c.UserRoles.Select(r => r.Role));
                query = query.Include(c => c.UserRoles.Select(r => r.User));
            }
            
            // Query It
            query = query.Where(c => c.UserId == userId);

            return await query.FirstOrDefaultAsync();
            
        }

        public async Task<User> GetUserByNameAsync(string name, bool includeUserRoles = false)
        {
            IQueryable<User> query = _context.Users;
            //Add user roles
            if (includeUserRoles)
            {
                query = query.Include(c => c.UserRoles.Select(r => r.Role));
                query = query.Include(c => c.UserRoles.Select(r => r.User));
            }

            // Query It
            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();

        }

        public async Task<Role> GetRoleByUserIdAsync(int userId, int roleId)
        {
            return await _context.UserRoles
          .Where(t => t.User.UserId == userId)
          .Select(t => t.Role)
          .Where(s => s != null && s.RoleId == roleId)
          .SingleOrDefaultAsync();
        }

        public async Task<Role[]> GetRolesByUserIdAsync(int userId)
        {
            IQueryable<Role> query = _context.UserRoles
              .Where(t => t.User.UserId == userId)
              .Select(t => t.Role)
              .Where(s => s != null)
              .Distinct();

            return await query.ToArrayAsync();
        }


    }
}
