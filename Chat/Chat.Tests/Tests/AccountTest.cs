using System.Web.Mvc;
using System.Web.Security;
using Chat.Controllers;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Tests.Tests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void RegisterSuccessTest()
        {
            var mock = new Mock<IAuthorizationService>();
            var accountController = new AccountController(mock.Object);

            var view = accountController.Register(new UserRegistration { Login = "John", Password = "pass" });

            mock.Verify(service => service.Register("John", "pass"), Times.Once());
            mock.Verify(service => service.Login("John", "pass"), Times.Once());
            Assert.IsInstanceOfType(view, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void RegisterUnsuccessTest()
        {
            var mock = new Mock<IAuthorizationService>();
            mock.Setup(service => service.Register("John", "pass")).Throws(new MembershipCreateUserException());            
            var accountController = new AccountController(mock.Object);
            
            var view = accountController.Register(new UserRegistration {Login = "John", Password = "pass"});

            mock.Verify(service => service.Login("John", "pass"), Times.Never());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
        }

        [TestMethod]
        public void LoginSuccessTest()
        {
            var mock = new Mock<IAuthorizationService>();
            mock.Setup(service => service.Login("John", "pass")).Returns(true);            
            var accountController = new AccountController(mock.Object);

            var view = accountController.Login(new UserLogin { Login = "John", Password = "pass" }, null);

            mock.Verify(service => service.Login("John", "pass"), Times.Once());
            Assert.IsInstanceOfType(view, typeof(RedirectResult));
        }

        [TestMethod]
        public void LoginUnsuccessTest()
        {
            var mock = new Mock<IAuthorizationService>();
            mock.Setup(service => service.Login("John", "pass")).Returns(false);            
            var accountController = new AccountController(mock.Object);

            var view = accountController.Login(new UserLogin { Login = "John", Password = "pass" }, null);

            mock.Verify(service => service.Login("John", "pass"), Times.Once());
            Assert.IsInstanceOfType(view, typeof(ViewResult));
        }
    }
}