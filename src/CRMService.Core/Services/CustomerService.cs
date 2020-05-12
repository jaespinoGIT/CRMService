using CRMService.Core.Domain.Entities;
using CRMService.Core.Domain.Entities.Enums;
using CRMService.Core.Exceptions;
using CRMService.Core.Exceptions.Services;
using CRMService.Core.Repositories;
using CRMService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomExceptionService _customExceptionService;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _customExceptionService = new CustomExceptionService();
        }

        public async Task<Customer> GetCustomerAsync(int customerId, bool full = false)
        {
            return await _customerRepository.GetCustomerAsync(customerId, full); ;
        }

        public async Task<List<Customer>> GetAllCustomersAsync(bool includeCustomerAudits = false)
        {
            return await _customerRepository.GetAllCustomersAsync(includeCustomerAudits);
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            if (customer == null) _customExceptionService.ThrowItemNotFoundException("Customer doesnt exists");

            _customerRepository.DeleteCustomer(customer);

            return await _customerRepository.SaveChangesAsync();

        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            if (await _customerRepository.GetCustomerByNameAsync(customer.Name) != null)
            {
                _customExceptionService.ThrowInvalidOperationException("Name already exists");
                return null;               
            }

            customer.CustomerAudits = new CustomerAudit[]
                {
                        new CustomerAudit
                        {
                            Date = DateTime.Now,
                            Operation = CustomerAuditOperationType.Create,
                            Customer = customer
                        }
                };

            _customerRepository.AddCustomer(customer);

            if (await _customerRepository.SaveChangesAsync())
                return customer;
            else
                _customExceptionService.ThrowInvalidOperationException("Error adding customer");

            return null;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            //customer.CustomerAudits = new CustomerAudit[]
            //        {
            //            new CustomerAudit
            //            {
            //                Date = DateTime.Now,
            //                Operation = CustomerAuditOperationType.Update,
            //                Customer = customer
            //            }
            //        };            

            if (await _customerRepository.SaveChangesAsync())
                return customer;
            else
                _customExceptionService.ThrowInvalidOperationException("Error updating user");

            return null;
        }   
    }
}
