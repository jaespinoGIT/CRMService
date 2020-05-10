using CRMService.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMService.Core.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetQueryable();
        void CreateCustomer(Customer customer);

        List<Customer> GetAll();

        Task<bool> SaveChangesAsync();

        // Customers
        void AddCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync(bool full = false);
        Task<Customer> GetCustomerAsync(int customerId, bool full = false);
        Task<Customer> GetCustomerByNameAsync(string name, bool full = false);

    }
}