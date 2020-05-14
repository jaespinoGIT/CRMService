
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Data.Entity;

using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;


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

        public void DeleteCustomer(Customer customer)
        {

            _context.Customers.Remove(customer);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
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
                query = query.Include(c => c.CustomerAudits.Select(r => r.Customer));

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

        public Task<Customer> GetCustomerAsync(int customerId, bool full = false)
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

    }
}
