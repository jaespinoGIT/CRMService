﻿using CRMService.Core.Domain.Entities;
using CRMService.Core.Domain.Entities.Enums;
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

        public CustomerService(ICustomerRepository customerRepository)                           
        {
            _customerRepository = customerRepository;          
        }

        public async Task<Customer> GetCustomerAsync(int customerId, bool full = false)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId, full);                  

            return customer;
        }

        public async Task<List<Customer>> GetAllCustomersAsync(bool includeCustomerAudits = false)
        {
            return await _customerRepository.GetAllCustomersAsync(includeCustomerAudits);
        }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            var customer = await _customerRepository.GetCustomerAsync(customerId);
            if (customer == null) return false;

            _customerRepository.DeleteCustomer(customer);

            return await _customerRepository.SaveChangesAsync();
       
        }

        public async Task<bool> AddCustomer(Customer customer)
        {      
            if (await _customerRepository.GetCustomerByNameAsync(customer.Name) != null)
            {
                return false;
                //ModelState.AddModelError("Name", "Name in use");   
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

            return await _customerRepository.SaveChangesAsync();         
        }

        public async Task<Customer> UpdateCustomer(int customerId, Customer customerUpdate)
        {

            var customer = await _customerRepository.GetCustomerAsync(customerId);
            if (customer == null) return null;

            customerUpdate.CustomerId = customerId;

            customer = customerUpdate;

            //var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId");
            customer.CustomerAudits = new CustomerAudit[]
                    {
                        new CustomerAudit
                        {
                            Date = DateTime.Now,
                            Operation = CustomerAuditOperationType.Update,
                            Customer = customer
                        }

                    };

            if (await _customerRepository.SaveChangesAsync())
            {
                return customer;
            }
            else
            {
                return  null;
            }
        }
    }
}