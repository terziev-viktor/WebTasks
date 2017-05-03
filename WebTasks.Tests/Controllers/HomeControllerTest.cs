using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebTasks.Controllers;
using Moq;
using System.Security.Principal;
using System.Web;

namespace WebTasks.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _controller;

        [TestInitialize]
        public void Init()
        {
            _controller = new HomeController();
        }

        [TestMethod]
        public void About()
        {
            // Act
            ViewResult result = _controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("WebTasks", result.ViewBag.Message);
        }

        [TestMethod]
        public void HomeController_RedirectsToAdminArea()
        {
            var homeController = new HomeController();

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole("Admin")).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            homeController.ControllerContext = controllerContextMock.Object;
            var result = homeController.Index();
            var resultType = result.GetType();

            Assert.AreEqual(resultType, typeof(RedirectResult));
            Assert.AreEqual((result as RedirectResult).Url, "/Admin/DailyTasks/Index");
        }

        [TestMethod]
        public void HomeController_RedirectsToUserArea()
        {
            var homeController = new HomeController();

            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole("Admin")).Returns(false);
            userMock.Setup(p => p.IsInRole("User")).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            homeController.ControllerContext = controllerContextMock.Object;
            var result = homeController.Index();
            var resultType = result.GetType();

            Assert.AreEqual(resultType, typeof(RedirectResult));
            Assert.AreEqual((result as RedirectResult).Url, "/User/DailyTasks/Index");
        }
    }
}
