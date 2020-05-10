using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Infrastructure.Data.EntityFramework.Repositories
{
    public class RoleRepository : IRoleRepository    
    {
       
        private readonly DataContext _context;


        private readonly DbSet<Role> _dbSet;

        public RoleRepository(DbSet<Role> dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<Role> GetQueryable()
        {
            return _dbSet;
        }

        public void CreateRole(Role role)
        {
            _dbSet.Add(role);
        }

        public List<Role> GetAll()
        {
            return _dbSet.AsQueryable().ToList();
        }

        public RoleRepository(DataContext context)
        {
            _context = context;
        }


        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
        }

        public void DeleteRole(Role role)
        {
            _context.Roles.Remove(role);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            IQueryable<Role> query = _context.Roles;         

            // Order It
            query = query.OrderByDescending(c => c.RoleName);

            return await query.ToListAsync();

        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            IQueryable<Role> query = _context.Roles;
            // Query It
            query = query.Where(c => c.RoleId == roleId);

            return await query.FirstOrDefaultAsync();

        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            IQueryable<Role> query = _context.Roles;
         
            // Query It
            query = query.Where(c => c.RoleName == name);

            return await query.FirstOrDefaultAsync();

        }
    }
}
