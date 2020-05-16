using AutoMapper;
using CRMService.Controllers;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services;
using CRMService.Core.Services.Interfaces;
using CRMService.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace CRMService.UnitTests.Controllers
{
    [TestClass]
    public class CustomersControllerTests
    {
        private List<Customer> customersList = new List<Customer>()
                {
                    new Customer() {CustomerId = 1, Name = "C1", Surname="S1"},
                    new Customer() {CustomerId = 2, Name = "C2", Surname="S2"},
                    new Customer() {CustomerId = 3, Name = "C3", Surname="S3"},
                };

        private CustomerModel customerModel = new CustomerModel()
                {
                   Name = "C4", Surname="S4"     
                };
        //private CustomerAudit customerAudit = new CustomerAudit()
        //{
        //    Date = DateTime.Now,
        //    Operation = Domain.Entities.Enums.CustomerAuditOperationType.Update
        //};
        private static MapperConfiguration mockMapper;
        private static Mock<ICustomerService> customerService;      
        private static IMapper _mapper; 

        [ClassInitialize]
        public static void SetupCustomerRepository(TestContext context)
        {   
            customerService = new Mock<ICustomerService>();    

            mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesMappingProfile());
            });

            _mapper = mockMapper.CreateMapper();
        }

        [TestMethod]
        public async Task GetAllCustomersAsync()
        { 
            customerService.Setup(x => x.GetAllCustomersAsync(false))
                  .Returns(Task.FromResult(customersList));  
                var controller = new CustomersController(customerService.Object, _mapper);  
                //act
                IHttpActionResult httpActionResult = await controller.Get();
                Assert.IsNotNull(httpActionResult);
                Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<IEnumerable<CustomerModel>>));

                var contentResult = httpActionResult as OkNegotiatedContentResult<IEnumerable<CustomerModel>>;

                Assert.AreEqual(customersList.Count(), contentResult.Content.Count());
                var mappedResult = _mapper.Map<IEnumerable<CustomerModel>>(contentResult.Content);
                Assert.IsNotNull(mappedResult);

            //var userMock = new Mock<IPrincipal>();
            //userMock.Setup(p => p.IsInRole("admin")).Returns(true);
            //userMock.SetupGet(p => p.Identity.Name).Returns("tester");
            //userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);

            //var requestContext = new Mock<HttpRequestContext>();
            //requestContext.Setup(x => x.Principal).Returns(userMock.Object);

            //var controller = new UsersController()
            //{
            //    RequestContext = requestContext.Object,
            //    Request = new HttpRequestMessage(),
            //    Configuration = new HttpConfiguration()
            //};
        }

        [TestMethod]
        [DataRow(1)]
        public async Task GetCustomerAsync(int customerId)
        {
            Customer customer = customersList.Find(c => c.CustomerId == customerId);
            customerService.Setup(x => x.GetCustomerAsync(customerId, false))
                  .Returns(Task.FromResult(customer));

            var controller = new CustomersController(customerService.Object, _mapper);
            //act
            IHttpActionResult httpActionResult = await controller.Get(customerId, false);
            Assert.IsNotNull(httpActionResult);
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<CustomerModel>));

            var contentResult = httpActionResult as OkNegotiatedContentResult<CustomerModel>;            
            var mappedResult = _mapper.Map<CustomerModel>(contentResult.Content);

            Assert.IsNotNull(mappedResult);
            Assert.AreEqual(customer.CustomerId, mappedResult.CustomerId);
            Assert.AreEqual(customer.Name, mappedResult.Name);
            Assert.AreEqual(customer.Surname, mappedResult.Surname);
        }

        [TestMethod]
        [DataRow(1)]
        public async Task DeleteCustomer(int customerId)
        {          
            customerService.Setup(x => x.DeleteCustomer(customerId)).Returns(Task.FromResult(true));

            var controller = new CustomersController(customerService.Object, _mapper);
            //act
            IHttpActionResult httpActionResult = await controller.Delete(customerId);
            Assert.IsNotNull(httpActionResult);
            Assert.IsInstanceOfType(httpActionResult, typeof(OkResult));         
        }

        [TestMethod]
        [DataRow(1)]
        public async Task UpdateCustomer(int customerId)
        {
            Customer customer = customersList.Find(c => c.CustomerId == customerId);
            customerService.Setup(x => x.GetCustomerForUpdateAsync(customerId))
                  .Returns(Task.FromResult(customer));

            customerService.Setup(x => x.UpdateCustomer(customer, null))
                .Returns(Task.FromResult(customer));

            var controller = new CustomersController(customerService.Object, _mapper);
            //act
            var httpActionResult = await controller.Put(customerId, customerModel);

            Assert.IsNotNull(httpActionResult);
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<CustomerModel>));

            var contentResult = httpActionResult as OkNegotiatedContentResult<CustomerModel>;
            var mappedResult = _mapper.Map<CustomerModel>(contentResult.Content);

            Assert.IsNotNull(mappedResult);
            Assert.AreEqual(customer.CustomerId, mappedResult.CustomerId);
            Assert.AreEqual(customer.Name, mappedResult.Name);
            Assert.AreEqual(customer.Surname, mappedResult.Surname);
        }


        [TestMethod]
        [DataRow(1)]
        public async Task GetPhoto(int customerId)
        {
            Customer customer = customersList.Find(c => c.CustomerId == customerId);
            Random rnd = new Random();
            Byte[] b = new Byte[10];
            rnd.NextBytes(b);
            customer.Photo = b;

            customerService.Setup(x => x.GetCustomerAsync(customerId, true))
                  .Returns(Task.FromResult(customer));

            var controller = new CustomersController(customerService.Object, _mapper);
            //act
            var result = await controller.Get(customerId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.IsNotNull(result.Content);
            Assert.IsNotNull(result.Content.Headers.ContentType);
        }
    }
}
