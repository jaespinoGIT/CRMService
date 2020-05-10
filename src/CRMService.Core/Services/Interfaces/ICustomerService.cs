using CRMService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> AddCustomer(Customer customer);
        Task<bool> DeleteCustomer(int customerId);

        Task<Customer> GetCustomerAsync(int customerId, bool full = false);

        Task<List<Customer>> GetAllCustomersAsync(bool includeCustomerAudits = false);

        Task<Customer> UpdateCustomer(int customerId, Customer customer);
    }
}
