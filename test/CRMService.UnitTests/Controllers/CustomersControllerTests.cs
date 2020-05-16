using AutoMapper;
using CRMService.Controllers;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Repositories;
using CRMService.Core.Services;
using CRMService.Models;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace CRMService.UnitTests.Controllers
{
    [TestClass]
    public class CustomersControllerTests
    {
        [TestMethod]
        public async Task GetAllCustomers()
        {           
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new EntitiesMappingProfile());
                });
                var mapper = mockMapper.CreateMapper();

                var _customerRepository = A.Fake<ICustomerRepository>();
                var service = new CustomerService(customerRepository: _customerRepository);
               
                var controller = new CustomersController(customerService: service, mapper: mapper);

                var customers = new List<Customer>()
                {
                    new Customer() {CustomerId = 1, Name = "C1", Surname="S1"},
                    new Customer() {CustomerId = 2, Name = "C2", Surname="S2"},
                    new Customer() {CustomerId = 3, Name = "C3", Surname="S3"},
                };

                var mock = customers.AsQueryable().BuildMock();
                A.CallTo(() => _customerRepository.GetQueryable()).Returns(mock);
                //act
                IHttpActionResult httpActionResult = await controller.Get();
                Assert.IsNotNull(httpActionResult);
                Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<IEnumerable<CustomerModel>>));

                var contentResult = httpActionResult as OkNegotiatedContentResult<IEnumerable<CustomerModel>>;

                Assert.AreEqual(customers.Count(), contentResult.Content.Count());

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
    }
}
