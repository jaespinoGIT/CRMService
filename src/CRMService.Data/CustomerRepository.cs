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

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            IQueryable<Customer> query = _context.Customers;                      

            // Order It
            query = query.OrderByDescending(c => c.Name);

            return await query.ToListAsync();

        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

    }
}
