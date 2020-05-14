using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMService.Core.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {     

        Task<List<Customer>> GetAllCustomersAsync(bool full = false);
        Task<Customer> GetCustomerAsync(int customerId, bool full = false);
        Task<Customer> GetCustomerByNameAsync(string name, bool full = false);

    }
}