using CRMService.Infrastructure.Data.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CRMService.Infrastructure.Data.EntityFramework.Repositories
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
        Task<List<Customer>> GetAllCustomersAsync(bool includeCustomerAudits = false);
        Task<Customer> GetCustomerAsync(int customerId, bool includeCustomerAudits = false);
        Task<Customer> GetCustomerByNameAsync(string name, bool includeCustomerAudits = false);

        // Customers audit
        void AddCustomerAudit(CustomerAudit customerAudit);
    }
}