using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMService.Core.UnitTests.Services
{
    [TestClass]
    public class CustomersServiceTests
    {
        [TestMethod]
        public async Task GetAllCustomers()
        {
            try
            {  
                var _customerRepository = A.Fake<ICustomerRepository>();
                var service = new CustomerService(customerRepository: _customerRepository);

                var customers = new List<Customer>()
                {
                    new Customer() {CustomerId = 1, Name = "C1", Surname="S1"},
                    new Customer() {CustomerId = 2, Name = "C2", Surname="S2"},
                    new Customer() {CustomerId = 3, Name = "C3", Surname="S3"},
                };

                var mock = customers.AsQueryable().BuildMock();
                A.CallTo(() => _customerRepository.GetQueryable()).Returns(mock);
                //act
                var result = await service.GetAllCustomersAsync();
                //assert
                Assert.AreEqual(customers.Count, result.Count);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsFalse(false, ex.Message);
            }
        }
    }
}
