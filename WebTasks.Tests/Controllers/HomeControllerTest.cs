using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebTasks.Controllers;

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
    }
}
