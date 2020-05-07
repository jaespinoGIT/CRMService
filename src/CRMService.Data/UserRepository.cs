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

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            IQueryable<User> query = _context.Users;                      

            // Order It
            query = query.OrderByDescending(c => c.Name);

            return await query.ToListAsync();

        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }
}
