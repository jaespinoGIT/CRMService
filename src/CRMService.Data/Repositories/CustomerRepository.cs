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

        public async Task<List<Customer>> GetAllCustomersAsync(bool includeCustomerAudits = false)
        {
            IQueryable<Customer> query = _context.Customers;

            if (includeCustomerAudits)
            {
                query = query.Include(c => c.CustomerAudits.Select(r => r.Customer));
                query = query.Include(c => c.CustomerAudits.Select(r => r.User));
            }

            query = query.OrderByDescending(c => c.Name);

            return await query.ToListAsync();

        }

        public async Task<Customer> GetCustomerAsync(int customerId, bool includeCustomerAudits = false)
        {
            IQueryable<Customer> query = _context.Customers
                 .Where(t => t.CustomerId == customerId);

            if (includeCustomerAudits)
            {
                query = query.Include(c => c.CustomerAudits.Select(r => r.Customer));
                query = query.Include(c => c.CustomerAudits.Select(r => r.User));
            }  

            return await query.FirstOrDefaultAsync();            
        }

        public async Task<Customer> GetCustomerByNameAsync(string name, bool includeCustomerAudits = false)
        {
            IQueryable<Customer> query = _context.Customers;
            //Add user audits
            if (includeCustomerAudits)
            {
                query = query.Include(c => c.CustomerAudits.Select(r => r.Customer));
                query = query.Include(c => c.CustomerAudits.Select(r => r.User));
            }

            // Query It
            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();

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
