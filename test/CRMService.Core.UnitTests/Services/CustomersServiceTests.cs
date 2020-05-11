using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services;
using CRMService.Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMService.Core.UnitTests.Services
{
    [TestClass]
    public class CustomersServiceTests
    {
        private List<Customer> customersList = new List<Customer>()
                {
                    new Customer() {CustomerId = 1, Name = "C1", Surname="S1"},
                    new Customer() {CustomerId = 2, Name = "C2", Surname="S2"},
                    new Customer() {CustomerId = 3, Name = "C3", Surname="S3"},
                };

        private static Mock<ICustomerRepository> customerRepository;

        [ClassInitialize]
        public static void SetupCustomerRepository(TestContext context)
        {
            customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.SaveChangesAsync())
                    .Returns(Task.FromResult(true));
        }

        [TestMethod]
        public async Task GetAllCustomers()
        {    
            customerRepository.Setup(x => x.GetAllCustomersAsync(false))
                   .Returns(Task.FromResult(customersList));

            var service = new CustomerService(customerRepository.Object);
            //act
            var result = await service.GetAllCustomersAsync();
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customersList.Count, result.Count);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task GetCustomer(int customerId)
        {
            Customer customer = customersList.Find(c => c.CustomerId == customerId);

           
            customerRepository.Setup(x => x.GetCustomerAsync(customerId, false))
                    .Returns(Task.FromResult(customer));

            var service = new CustomerService(customerRepository.Object);

            var result = await service.GetCustomerAsync(customerId);
            //assert
            Assert.IsNotNull(result);          
           
            Assert.AreEqual(customer.CustomerId, result.CustomerId);
            Assert.AreEqual(customer.Name, result.Name);
            Assert.AreEqual(customer.Surname, result.Surname);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task DeleteCustomer(int customerId)
        {
            Customer customer = customersList.Find(c => c.CustomerId == customerId);

           
            customerRepository.Setup(x => x.GetCustomerAsync(customerId, false))
               .Returns(Task.FromResult(customer));
          
            customerRepository.Setup(x => x.DeleteCustomer(customer));

            var service = new CustomerService(customerRepository.Object);            
     
            var result = await service.DeleteCustomer(customer.CustomerId);
            customerRepository.Verify(r => r.DeleteCustomer(customer));

            Assert.IsNotNull(result);
            Assert.IsTrue(result);      
        }

        [TestMethod]
        [DataRow(1)]
        public async Task AddCustomer(int customerId)
        {
            Customer customer = customersList.Find(c => c.CustomerId == customerId);
          
            customerRepository.Setup(x => x.GetCustomerByNameAsync(customer.Name, false));            
           
            customerRepository.Setup(x => x.AddCustomer(customer));

            var service = new CustomerService(customerRepository.Object);

            var result = await service.AddCustomer(customer);
            customerRepository.Verify(r => r.AddCustomer(customer));

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }
    }
}
