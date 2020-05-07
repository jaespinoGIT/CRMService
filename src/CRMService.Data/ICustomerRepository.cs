using CRMService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CRMService.Data
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
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(int customerId);     
    }
}