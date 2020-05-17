using CRMService.Controllers;
using CRMService.Core.Domain.Entities;
using CRMService.Models;
using CRMService.Models.Binding;
using CRMService.Models.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace CRMService.UnitTests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private static List<User> userList = new List<User>()
                {
                    new User() {Id = "1", UserName = "C1", PasswordHash = new PasswordHasher().HashPassword("testpass1")},
                    new User() {Id = "2", UserName = "C2", PasswordHash = new PasswordHasher().HashPassword("testpass2")},
                    new User() {Id = "3", UserName = "C3", PasswordHash = new PasswordHasher().HashPassword("testpass3")},
                };
        private static User validTestUser = new User()
        {
            Id = "1",
            UserName = "user",
            PasswordHash = new PasswordHasher().HashPassword("testpass")
        };

        private static CreateUserBindingModel userBindingModel = new CreateUserBindingModel()
        {            
            Username = "user",
            Email = "user@gmail.com",
            FirstName = "first",
            LastName = "last",
            Password = new PasswordHasher().HashPassword("testpass")
        };

        private static UsersController usersController;
        private static UserModel userModel = new UserModel()
        {
            Url = "api/users/1",
            Id = "1",
            UserName = "user",
            FullName = "user user",
            Roles = new List<string>() { "Admin" },
            Claims = null
        };              

        [ClassInitialize]
        public static void SetupCustomerRepository(TestContext context)
        {
            usersController = new UsersController();
            MockUserManager(usersController);
            MockModelFactory(usersController);
        }

        private static void MockUserManager(UsersController c)
        {
            var hpw = new PasswordHasher().HashPassword("testpass");

            var us = new Mock<IUserStore<User>>(MockBehavior.Strict);
            us.Setup(p => p.FindByNameAsync("user")).ReturnsAsync(validTestUser);
            us.Setup(p => p.FindByIdAsync(validTestUser.Id)).ReturnsAsync(validTestUser);    
          
            us.As<IUserPasswordStore<User>>()
                .Setup(p => p.GetPasswordHashAsync(It.IsAny<User>())).ReturnsAsync(hpw);
            var aum = new ApplicationUserManager(us.Object);

            c.AppUserManager = aum;
        }

        private static void MockModelFactory(UsersController c)
        {
            var us = new Mock<IModelFactory>(MockBehavior.Strict);
            us.Setup(p => p.Create(validTestUser)).Returns(userModel);

            c.TheModelFactory = us.Object;
        }

        [TestMethod]
        [DataRow("user")]
        public async Task GetUserByName(string username)
        {
            IHttpActionResult httpActionResult = await usersController.GetUserByName(username);
            Assert.IsNotNull(httpActionResult);
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<UserModel>));

            var contentResult = httpActionResult as OkNegotiatedContentResult<UserModel>;
            var mappedResult = contentResult.Content;

            Assert.IsNotNull(mappedResult);
            Assert.AreEqual(validTestUser.Id, mappedResult.Id);
            Assert.AreEqual(validTestUser.UserName, mappedResult.UserName);
        }

        [TestMethod]
        [DataRow("1")]
        public async Task GetUserById(string id)
        {           
            IHttpActionResult httpActionResult = await usersController.Get(id);
            Assert.IsNotNull(httpActionResult);
            Assert.IsInstanceOfType(httpActionResult, typeof(OkNegotiatedContentResult<UserModel>));

            var contentResult = httpActionResult as OkNegotiatedContentResult<UserModel>;
            var mappedResult = contentResult.Content;

            Assert.IsNotNull(mappedResult);
            Assert.AreEqual(validTestUser.Id, mappedResult.Id);
            Assert.AreEqual(validTestUser.UserName, mappedResult.UserName);
        }
    }
}
