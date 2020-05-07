using CRMService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace CRMService.Data
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

        public IQueryable<Customer> GetQueryable()
        {
            return _dbSet;
        }

        public void CreateCustomer(Customer customer)
        {
            _dbSet.Add(customer);
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

        public void AddCustomerAudit(CustomerAudit customerAudit)
        {
            _context.CustomerAudits.Add(customerAudit);
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            IQueryable<Customer> query = _context.Customers;  
          
            query = query.OrderByDescending(c => c.Name);

            return await query.ToListAsync();

        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<List<CustomerAudit>> GetCustomerAuditByUserIdAsync(int customerId)
        {
            IQueryable<CustomerAudit> query = _context.CustomerAudits
                   .Where(t => t.Customer.CustomerId == customerId) 
                   .OrderByDescending(s => s.Date)
                   .Distinct();

            return await query.ToListAsync();
        }

    }
}
