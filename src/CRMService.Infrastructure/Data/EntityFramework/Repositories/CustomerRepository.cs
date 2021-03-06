﻿
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Data.Entity;

using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace CRMService.Infrastructure.Data.EntityFramework.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;


        private readonly DbSet<Customer> _dbSet;

        public CustomerRepository()
        {

        }
        public CustomerRepository(DbSet<Customer> dbSet)
        {
            _dbSet = dbSet;
        }

        public void CreateCustomer(Customer customer)
        {
            _dbSet.Add(customer);
        }

        public IQueryable<Customer> GetQueryable()
        {
            return _dbSet;
        }
        public List<Customer> GetAll()
        {
            return _dbSet.AsQueryable().ToList();
        }

        public CustomerRepository(DataContext context)
        {
            _context = context;
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void AddCustomerAudit(CustomerAudit customerAudit, string userId)
        {     
            User user = _context.Users
                 .Where(t => t.Id == userId)
                 .FirstOrDefault();

            if (user != null)
                customerAudit.User = user; 
          
            _context.CustomerAudits.Add(customerAudit);
        }

        public void DeleteCustomer(Customer customer)
        {
            //_context.Customers.Attach(customer);           
            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges
                // Only return success if at least one row was changed
                return (await _context.SaveChangesAsync()) > 0;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                var innerException = (SqlException)ex.InnerException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                {
                    // log the error;
                }
                throw;

            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync(bool full = false)
        {
            IQueryable<Customer> query = _context.Customers;
            query = query.OrderByDescending(c => c.Name);

            if (full)
                return await query.ToListAsync();
            else
            {
                var lista = await query.Select(x => new
                {
                    CustomerId = x.CustomerId,
                    Name = x.Name,
                    Surname = x.Surname
                }).ToListAsync();

                List<Customer> listaCustomers = new List<Customer>();

                if (lista != null) 
                    foreach (var item in lista)                   
                        listaCustomers.Add(new Customer() { CustomerId = item.CustomerId, Name = item.Name, Surname = item.Surname });

                return listaCustomers;
            }
        }

        private async Task<Customer> GetCustomerAsync(IQueryable<Customer> query, bool full)
        {
            if (full)
            {
                query = query.Include(c => c.CustomerAudits.Select(r => r.Customer));
                query = query.Include(c => c.CustomerAudits.Select(r => r.User));

                return await query.FirstOrDefaultAsync();
            }
            else
            {
                var cust = await query.Select(x => new
                {
                    CustomerId = x.CustomerId,
                    Name = x.Name,
                    Surname = x.Surname
                }).FirstOrDefaultAsync();

                return cust != null ? new Customer() { CustomerId = cust.CustomerId, Name = cust.Name, Surname = cust.Surname } : null ;
            }
        }

        public Task<Customer> GetCustomerAsync(int customerId, bool full)
        {
            IQueryable<Customer> query = _context.Customers
                 .Where(t => t.CustomerId == customerId);

            return GetCustomerAsync(query, full);
        }

        public Task<Customer> GetCustomerByNameAsync(string name, bool full = false)
        {
            IQueryable<Customer> query = _context.Customers
                        .Where(c => c.Name == name);

            return GetCustomerAsync(query, full);
        }

        public Task<Customer> GetCustomerForUpdateAsync(int customerId)
        {
            IQueryable<Customer> query = _context.Customers
                 .Where(t => t.CustomerId == customerId);

            return query.FirstOrDefaultAsync();
        }

    }
}
