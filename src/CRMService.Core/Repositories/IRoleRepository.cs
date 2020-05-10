using CRMService.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMService.Core.Repositories
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetQueryable();
        void CreateRole(Role role);

        List<Role> GetAll();

        Task<bool> SaveChangesAsync();

        // Roles
        void AddRole(Role role);
        void DeleteRole(Role role);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        Task<Role> GetRoleByNameAsync(string name);       
    }
}